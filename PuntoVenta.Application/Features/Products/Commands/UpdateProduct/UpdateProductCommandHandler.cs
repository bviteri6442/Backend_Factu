using MediatR;
using PuntoVenta.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Obtener el producto existente
                var producto = await _unitOfWork.Productos.GetByIdAsync(request.Id);
                if (producto == null)
                {
                    throw new Exception("Producto no encontrado");
                }

                // Actualizar campos
                if (!string.IsNullOrEmpty(request.Nombre))
                    producto.Nombre = request.Nombre;

                if (!string.IsNullOrEmpty(request.Descripcion))
                    producto.Descripcion = request.Descripcion;

                if (request.PrecioVenta > 0)
                    producto.Precio = request.PrecioVenta;

                if (request.PrecioCompra > 0)
                    producto.PrecioCompra = request.PrecioCompra;

                if (request.Stock.HasValue)
                    producto.Stock = request.Stock.Value;

                if (request.StockMinimo.HasValue)
                    producto.StockMinimo = request.StockMinimo.Value;

                producto.FechaActualizacion = DateTime.UtcNow;

                await _unitOfWork.Productos.UpdateAsync(producto);
                await _unitOfWork.SaveChangesAsync();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el producto: {ex.Message}");
            }
        }
    }
}
