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
                throw new Exception("La factura debe tener al menos un producto");
            }

            // Validar que los productos existen y tienen stock
            decimal subtotal = 0;
            var detallesVenta = new List<DetalleVenta>();

            // Get cliente info if provided
            string clienteNombre = string.Empty;
            string clienteDocumento = string.Empty;
            if (request.ClienteId.HasValue)
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.ClienteId.Value);
                if (cliente != null)
                {
                    clienteNombre = cliente.Nombre;
                    clienteDocumento = cliente.Documento;
                }
            }

            // Get usuario info
            string usuarioNombre = string.Empty;
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(request.UsuarioId);
            if (usuario != null)
            {
                usuarioNombre = usuario.Nombre;
            }

            foreach (var detalle in request.Detalles)
            {
                var producto = await _unitOfWork.Productos.GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                {
                    throw new Exception($"El producto con ID {detalle.ProductoId} no existe");
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para {producto.Nombre}. Disponible: {producto.Stock}, Solicitado: {detalle.Cantidad}");
                }

                // Calcular total del detalle
                decimal totalDetalle = (detalle.PrecioUnitario * detalle.Cantidad) - detalle.Descuento;
                subtotal += totalDetalle;

                // Crear detalle de venta
                var detalleVenta = new DetalleVenta
                {
                    ProductoId = producto.Id,
                    ProductoNombre = producto.Nombre,
                    CodigoBarra = producto.Codigo,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Descuento = detalle.Descuento,
                    Total = totalDetalle
                };

                detallesVenta.Add(detalleVenta);
            }

            // Crear la factura
            var factura = new Factura
            {
                NumeroFactura = await _unitOfWork.Facturas.GenerarNumeroFacturaAsync(),
                FechaVenta = DateTime.UtcNow,
                UsuarioId = request.UsuarioId,
                UsuarioNombre = usuarioNombre,
                ClienteId = request.ClienteId,
                ClienteNombre = clienteNombre,
                ClienteDocumento = clienteDocumento,
                Subtotal = subtotal,
                PorcentajeIVA = 12m,
                TotalImpuesto = subtotal * 0.12m,
                TotalVenta = subtotal + (subtotal * 0.12m),
                Estado = "Completada",
                Observaciones = request.Observaciones ?? string.Empty,
                Detalles = detallesVenta
            };

            try
            {
                // Guardar factura
                var facturaId = await _unitOfWork.Facturas.AddAsync(factura);

                // Descontar stock de productos
                foreach (var detalle in detallesVenta)
                {
                    await _unitOfWork.Productos.UpdateStockAsync(detalle.ProductoId, -detalle.Cantidad);
                }

                await _unitOfWork.SaveChangesAsync();

                return facturaId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la factura: {ex.Message}");
            }
        }
    }
}
