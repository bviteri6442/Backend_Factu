using MediatR;

namespace PuntoVenta.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}
