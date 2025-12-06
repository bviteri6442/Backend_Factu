using MediatR;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validar que no exista producto con el mismo código
                var productoExistente = await _unitOfWork.Productos.GetByCodigoBarraAsync(request.Codigo ?? string.Empty);
                if (productoExistente != null)
                {
                    throw new Exception("Ya existe un producto con este código");
                }

                // Crear nuevo producto
                var nuevoProducto = new Product
                {
                    Nombre = request.Nombre ?? string.Empty,
                    Codigo = request.Codigo ?? string.Empty,
                    Precio = request.Precio,
                    PrecioCompra = request.PrecioCompra,
                    Stock = request.StockInicial,
                    StockMinimo = 10,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                await _unitOfWork.Productos.AddAsync(nuevoProducto);
                await _unitOfWork.SaveChangesAsync();

                return nuevoProducto.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el producto: {ex.Message}");
            }
        }
    }
}
