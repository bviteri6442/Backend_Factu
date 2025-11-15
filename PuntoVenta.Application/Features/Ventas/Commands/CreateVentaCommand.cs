using MediatR;
using PuntoVenta.Application.DTOs;

namespace PuntoVenta.Application.Features.Ventas.Commands
{
    public class CreateVentaCommand : IRequest<int>
    {
        public string? UsuarioId { get; set; }
        public int? ClienteId { get; set; }
        public List<CreateDetalleVentaDto> Detalles { get; set; } = new List<CreateDetalleVentaDto>();
        public string? Observaciones { get; set; }
    }
}
