using MediatR;

namespace PuntoVenta.Application.Features.Ventas.Commands
{
    public class UpdateVentaCommand : IRequest<bool>
    {
        public int VentaId { get; set; }
        public string? Estado { get; set; }
        public string? Observaciones { get; set; }
    }
}
