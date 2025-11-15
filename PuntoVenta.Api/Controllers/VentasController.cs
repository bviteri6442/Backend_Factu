using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Features.Ventas.Commands;
using PuntoVenta.Application.Features.Ventas.Queries;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PuntoVenta.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones de ventas (facturas)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VentasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todas las ventas con filtros opcionales
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<VentaResponseDto>>> GetVentas(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string usuarioId,
            [FromQuery] int? clienteId,
            [FromQuery] string estado)
        {
            try
            {
                var query = new GetVentasQuery
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    UsuarioId = usuarioId,
                    ClienteId = clienteId,
                    Estado = estado
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una venta específica por ID con todos sus detalles
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDetailResponseDto>> GetVentaById(int id)
        {
            try
            {
                var query = new GetVentaByIdQuery { VentaId = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva venta (factura)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> CreateVenta([FromBody] CreateVentaDto createVentaDto)
        {
            try
            {
                // Obtener ID del usuario desde el token JWT
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Unauthorized(new { message = "Usuario no autenticado" });
                }

                var command = new CreateVentaCommand
                {
                    UsuarioId = usuarioId,
                    ClienteId = createVentaDto.ClienteId,
                    Detalles = createVentaDto.Detalles,
                    Observaciones = createVentaDto.Observaciones
                };

                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetVentaById), new { id = result }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza el estado y observaciones de una venta existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<bool>> UpdateVenta(int id, [FromBody] UpdateVentaCommand updateCommand)
        {
            try
            {
                updateCommand.VentaId = id;
                var result = await _mediator.Send(updateCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una venta (solo si está en estado Cancelada o Anulada)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<bool>> DeleteVenta(int id)
        {
            try
            {
                var command = new DeleteVentaCommand { VentaId = id };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
