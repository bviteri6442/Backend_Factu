using MediatR;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Features.Ventas.Queries
{
    public class GetVentasQueryHandler : IRequestHandler<GetVentasQuery, List<VentaResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetVentasQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VentaResponseDto>> Handle(GetVentasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ventas = await _unitOfWork.Ventas.GetAllAsync();

                // Aplicar filtros
                var ventasFiltradas = ventas.AsQueryable();

                if (request.FechaInicio.HasValue)
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.FechaVenta >= request.FechaInicio.Value);
                }

                if (request.FechaFin.HasValue)
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.FechaVenta <= request.FechaFin.Value);
                }

                if (!string.IsNullOrEmpty(request.UsuarioId))
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.UsuarioId == request.UsuarioId);
                }

                if (request.ClienteId.HasValue)
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.ClienteId == request.ClienteId.Value);
                }

                if (!string.IsNullOrEmpty(request.Estado))
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.Estado == request.Estado);
                }

                var resultado = ventasFiltradas
                    .Select(v => new VentaResponseDto
                    {
                        VentaId = v.Id,
                        NumeroFactura = v.NumeroFactura,
                        FechaVenta = v.FechaVenta,
                        UsuarioId = v.UsuarioId,
                        UsuarioNombre = null,
                        ClienteId = v.ClienteId,
                        ClienteNombre = v.Cliente != null ? v.Cliente.Nombre : null,
                        Subtotal = v.Subtotal,
                        PorcentajeIVA = v.PorcentajeIVA,
                        TotalImpuesto = v.TotalImpuesto,
                        TotalVenta = v.TotalVenta,
                        Estado = v.Estado,
                        Observaciones = v.Observaciones
                    })
                    .OrderByDescending(v => v.FechaVenta)
                    .ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener ventas: {ex.Message}");
            }
        }
    }
}
