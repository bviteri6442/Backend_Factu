using MediatR;

namespace PuntoVenta.Application.Features.Ventas.Commands
{
    public class DeleteVentaCommand : IRequest<bool>
    {
        public int VentaId { get; set; }
    }
}
