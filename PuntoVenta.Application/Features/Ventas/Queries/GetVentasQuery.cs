using MediatR;
using System;
using System.Collections.Generic;
using PuntoVenta.Application.DTOs;

namespace PuntoVenta.Application.Features.Ventas.Queries
{
    public class GetVentasQuery : IRequest<List<VentaResponseDto>>
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? UsuarioId { get; set; }
        public int? ClienteId { get; set; }
        public string? Estado { get; set; }
    }
}
