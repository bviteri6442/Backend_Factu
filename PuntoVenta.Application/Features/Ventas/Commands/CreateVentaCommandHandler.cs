using MediatR;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Features.Ventas.Commands
{
    public class CreateVentaCommandHandler : IRequestHandler<CreateVentaCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateVentaCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateVentaCommand request, CancellationToken cancellationToken)
        {
            // Validar que hay detalles
            if (request.Detalles == null || !request.Detalles.Any())
            {
                throw new Exception("La venta debe tener al menos un producto");
            }

            // Validar que los productos existen y tienen stock
            decimal subtotal = 0;
            var detallesVenta = new List<DetalleVenta>();

            foreach (var detalle in request.Detalles)
            {
                var producto = await _unitOfWork.Productos.GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                {
                    throw new Exception($"El producto con ID {detalle.ProductoId} no existe");
                }

                if (producto.StockActual < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para {producto.Nombre}. Disponible: {producto.StockActual}, Solicitado: {detalle.Cantidad}");
                }

                // Calcular total del detalle
                decimal totalDetalle = (detalle.PrecioUnitario * detalle.Cantidad) - detalle.Descuento;
                subtotal += totalDetalle;

                // Crear detalle de venta
                var detalleVenta = new DetalleVenta
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Descuento = detalle.Descuento,
                    Total = totalDetalle
                };

                detallesVenta.Add(detalleVenta);
            }

            // Crear la venta
            var venta = new Venta
            {
                NumeroFactura = await _unitOfWork.Ventas.GenerarNumeroFacturaAsync(),
                FechaVenta = DateTime.UtcNow,
                UsuarioId = request.UsuarioId,
                ClienteId = request.ClienteId,
                Subtotal = subtotal,
                PorcentajeIVA = 19m,
                TotalImpuesto = subtotal * 0.19m,
                TotalVenta = subtotal + (subtotal * 0.19m),
                Estado = "Completada",
                Observaciones = request.Observaciones,
                Detalles = detallesVenta
            };

            // Operaciones en transacción
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Guardar venta (AddAsync internamente hace SaveChanges pero participará en la transacción)
                var ventaId = await _unitOfWork.Ventas.AddAsync(venta);

                // Descontar stock de productos
                foreach (var detalle in request.Detalles)
                {
                    await _unitOfWork.Productos.UpdateStockAsync(detalle.ProductoId, detalle.Cantidad);
                }

                // Commit
                await _unitOfWork.CommitTransactionAsync();

                return ventaId;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error al crear la venta: {ex.Message}");
            }
        }
    }
}
