using MediatR;

namespace PuntoVenta.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public string? Nombre { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioCompra { get; set; }
        public int StockInicial { get; set; }
        public int StockMinimo { get; set; }
    }
}
