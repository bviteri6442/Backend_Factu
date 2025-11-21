using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar registros de errores del sistema
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class ErrorLogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ErrorLogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtiene todos los errores registrados
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetErrorLogs(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string nivelSeveridad,
            [FromQuery] bool? revisado)
        {
            try
            {
                var errorLogs = await _unitOfWork.ErrorLogs.GetAllAsync();

                var filtrados = errorLogs.AsQueryable();

                if (fechaInicio.HasValue)
                {
                    filtrados = filtrados.Where(e => e.FechaError >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    filtrados = filtrados.Where(e => e.FechaError <= fechaFin.Value);
                }

                if (!string.IsNullOrEmpty(nivelSeveridad))
                {
                    filtrados = filtrados.Where(e => e.NivelSeveridad == nivelSeveridad);
                }

                if (revisado.HasValue)
                {
                    filtrados = filtrados.Where(e => e.Revisado == revisado.Value);
                }

                var resultado = filtrados
                    .OrderByDescending(e => e.FechaError)
                    .Select(e => new
                    {
                        Id = e.Id,
                        e.TipoError,
                        e.Mensaje,
                        e.Pantalla,
                        e.NivelSeveridad,
                        e.FechaError,
                        e.Revisado,
                        UsuarioId = e.UsuarioId,
                        UsuarioNombre = (string?)null
                    })
                    .ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un error específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetErrorLogById(string id) // Changed from int to string
        {
            try
            {
                var errorLog = await _unitOfWork.ErrorLogs.GetByIdAsync(id);
                if (errorLog == null)
                {
                    return NotFound(new { message = $"Error con ID {id} no encontrado" });
                }

                var resultado = new
                {
                    Id = errorLog.Id,
                    errorLog.TipoError,
                    errorLog.Mensaje,
                    errorLog.StackTrace,
                    errorLog.Fuente,
                    errorLog.NumeroLinea,
                    errorLog.Pantalla,
                    errorLog.Evento,
                    errorLog.NivelSeveridad,
                    errorLog.FechaError,
                    errorLog.Revisado,
                    UsuarioId = errorLog.UsuarioId,
                    UsuarioNombre = (string?)null
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Marca un error como revisado
        /// </summary>
        [HttpPut("{id}/revisar")]
        public async Task<ActionResult<bool>> MarcarComoRevisado(string id) // Changed from int to string
        {
            try
            {
                var errorLog = await _unitOfWork.ErrorLogs.GetByIdAsync(id);
                if (errorLog == null)
                {
                    return NotFound(new { message = $"Error con ID {id} no encontrado" });
                }

                errorLog.Revisado = true;
                await _unitOfWork.ErrorLogs.UpdateAsync(errorLog);
                await _unitOfWork.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un resumen de errores por tipo
        /// </summary>
        [HttpGet("resumen/por-tipo")]
        public async Task<ActionResult<dynamic>> GetErroresPorTipo()
        {
            try
            {
                var errorLogs = await _unitOfWork.ErrorLogs.GetAllAsync();
                var resumen = errorLogs
                    .GroupBy(e => e.TipoError)
                    .Select(g => new
                    {
                        TipoError = g.Key,
                        Cantidad = g.Count(),
                        UltimoCometido = g.Max(e => e.FechaError)
                    })
                    .OrderByDescending(e => e.Cantidad)
                    .ToList();

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene errores por nivel de severidad
        /// </summary>
        [HttpGet("resumen/por-severidad")]
        public async Task<ActionResult<dynamic>> GetErroresPorSeveridad()
        {
            try
            {
                var errorLogs = await _unitOfWork.ErrorLogs.GetAllAsync();
                var resumen = errorLogs
                    .GroupBy(e => e.NivelSeveridad)
                    .Select(g => new
                    {
                        NivelSeveridad = g.Key,
                        Cantidad = g.Count(),
                        Revisados = g.Count(e => e.Revisado),
                        Pendientes = g.Count(e => !e.Revisado)
                    })
                    .ToList();

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina errores más antiguos de 90 días
        /// </summary>
        [HttpDelete("limpiar")]
        public async Task<ActionResult<int>> LimpiarErroresAntiguos()
        {
            try
            {
                var errorLogs = await _unitOfWork.ErrorLogs.GetAllAsync();
                var fechaLimite = DateTime.UtcNow.AddDays(-90);
                var erroresEliminar = errorLogs.Where(e => e.FechaError < fechaLimite).ToList();

                foreach (var error in erroresEliminar)
                {
                    await _unitOfWork.ErrorLogs.DeleteAsync(error.Id); // error.Id is now string
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(new { message = $"Se eliminaron {erroresEliminar.Count} errores antiguos" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
