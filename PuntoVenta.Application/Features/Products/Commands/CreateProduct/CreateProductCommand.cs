using MediatR;

namespace PuntoVenta.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public string? Nombre { get; set; }
        public string? CodigoBarra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockInicial { get; set; }
    }
}
