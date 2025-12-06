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
                var facturas = await _unitOfWork.Facturas.GetAllAsync();

                Console.WriteLine($"[DEBUG] Facturas obtenidas: {facturas?.Count() ?? 0}");
                
                if (facturas == null || !facturas.Any())
                {
                    Console.WriteLine("[DEBUG] No se encontraron facturas en la base de datos");
                    return new List<VentaResponseDto>();
                }

                // Apply filters
                var query = facturas.AsEnumerable();

                if (request.FechaInicio.HasValue)
                {
                    query = query.Where(v => v.FechaVenta >= request.FechaInicio.Value);
                }

                if (request.FechaFin.HasValue)
                {
                    query = query.Where(v => v.FechaVenta <= request.FechaFin.Value);
                }

                if (request.UsuarioId.HasValue)
                {
                    query = query.Where(v => v.UsuarioId == request.UsuarioId.Value);
                }

                if (!string.IsNullOrEmpty(request.Estado))
                {
                    query = query.Where(v => v.Estado == request.Estado);
                }

                // Map to DTO
                var resultado = query.Select(f => new VentaResponseDto
                {
                    VentaId = f.Id,
                    NumeroFactura = f.NumeroFactura,
                    FechaVenta = f.FechaVenta,
                    UsuarioId = f.UsuarioId,
                    UsuarioNombre = f.UsuarioNombre,
                    ClienteId = f.ClienteId,
                    ClienteNombre = f.ClienteNombre,
                    Subtotal = f.Subtotal,
                    PorcentajeIVA = f.PorcentajeIVA,
                    TotalImpuesto = f.TotalImpuesto,
                    TotalVenta = f.TotalVenta,
                    Estado = f.Estado,
                    Observaciones = f.Observaciones
                }).ToList();

                Console.WriteLine($"[DEBUG] Facturas mapeadas a DTO: {resultado.Count}");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener facturas: {ex.Message}");
            }
        }
    }
}
