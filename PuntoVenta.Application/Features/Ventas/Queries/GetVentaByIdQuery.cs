using MediatR;
using System.Collections.Generic;
using PuntoVenta.Application.DTOs;

namespace PuntoVenta.Application.Features.Ventas.Queries
{
    public class GetVentaByIdQuery : IRequest<VentaDetailResponseDto>
    {
        public int VentaId { get; set; }
    }
}
