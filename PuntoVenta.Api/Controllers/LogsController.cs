using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.Interfaces;
using System.Text;

namespace PuntoVenta.Api.Controllers
{
    /// <summary>
    /// Controlador unificado para auditoría del sistema
    /// Gestiona tanto intentos de login como logs de errores
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class LogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Intentos de Login

        /// <summary>
        /// Obtiene todos los intentos de login con filtros opcionales
        /// </summary>
        [HttpGet("intentos-login")]
        public async Task<IActionResult> GetIntentosLogin(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] bool? exitoso,
            [FromQuery] string? usuario)
        {
            try
            {
                var intentos = await _unitOfWork.IntentosLogin.GetAllAsync();
                var query = intentos.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(i => i.FechaIntento >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(i => i.FechaIntento <= fechaFin.Value);

                if (exitoso.HasValue)
                    query = query.Where(i => i.Exitoso == exitoso.Value);

                if (!string.IsNullOrEmpty(usuario))
                    query = query.Where(i => i.NombreUsuario.Contains(usuario));

                var resultado = query
                    .OrderByDescending(i => i.FechaIntento)
                    .Select(i => new
                    {
                        i.Id,
                        i.NombreUsuario,
                        i.Exitoso,
                        i.IpAddress,
                        i.UserAgent,
                        i.FechaIntento,
                        i.MensajeError
                    })
                    .ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de intentos de login
        /// </summary>
        [HttpGet("intentos-login/estadisticas")]
        public async Task<IActionResult> GetEstadisticasIntentosLogin()
        {
            try
            {
                var intentos = await _unitOfWork.IntentosLogin.GetAllAsync();
                var lista = intentos.ToList();

                var estadisticas = new
                {
                    totalIntentos = lista.Count,
                    intentosExitosos = lista.Count(i => i.Exitoso),
                    intentosFallidos = lista.Count(i => !i.Exitoso),
                    intentosHoy = lista.Count(i => i.FechaIntento.Date == DateTime.UtcNow.Date),
                    intentosFallidosHoy = lista.Count(i => !i.Exitoso && i.FechaIntento.Date == DateTime.UtcNow.Date),
                    intentosSemana = lista.Count(i => i.FechaIntento >= DateTime.UtcNow.AddDays(-7)),
                    intentosMes = lista.Count(i => i.FechaIntento >= DateTime.UtcNow.AddMonths(-1)),
                    usuariosConIntentosFallidos = lista
                        .Where(i => !i.Exitoso)
                        .GroupBy(i => i.NombreUsuario)
                        .Select(g => new { usuario = g.Key, cantidad = g.Count() })
                        .OrderByDescending(x => x.cantidad)
                        .Take(5),
                    ultimosIntentos = lista
                        .OrderByDescending(i => i.FechaIntento)
                        .Take(10)
                        .Select(i => new
                        {
                            i.NombreUsuario,
                            i.Exitoso,
                            i.FechaIntento,
                            i.IpAddress
                        })
                };

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Genera reporte PDF de intentos de login
        /// </summary>
        [HttpGet("intentos-login/pdf")]
        public async Task<IActionResult> GenerarPDFIntentosLogin([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var intentos = await _unitOfWork.IntentosLogin.GetAllAsync();
                var query = intentos.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(i => i.FechaIntento >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(i => i.FechaIntento <= fechaFin.Value);

                var lista = query.OrderByDescending(i => i.FechaIntento).ToList();
                var html = GenerarHTMLIntentosLogin(lista);

                return Ok(new
                {
                    html = html,
                    nombreArchivo = $"Intentos_Login_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        #endregion

        #region Logs de Errores

        /// <summary>
        /// Obtiene todos los logs de errores con filtros opcionales
        /// </summary>
        [HttpGet("errores")]
        public async Task<IActionResult> GetErrorLogs(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string? nivel,
            [FromQuery] bool? revisado)
        {
            try
            {
                var errores = await _unitOfWork.ErrorLogs.GetAllAsync();
                var query = errores.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(e => e.Fecha >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(e => e.Fecha <= fechaFin.Value);

                if (!string.IsNullOrEmpty(nivel))
                    query = query.Where(e => e.Nivel == nivel);

                if (revisado.HasValue)
                    query = query.Where(e => e.Revisado == revisado.Value);

                var resultado = query
                    .OrderByDescending(e => e.Fecha)
                    .Select(e => new
                    {
                        e.Id,
                        e.TipoError,
                        e.Mensaje,
                        e.Origen,
                        e.Nivel,
                        e.Fecha,
                        e.Revisado,
                        e.UsuarioId,
                        e.Notas
                    })
                    .ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de logs de errores
        /// </summary>
        [HttpGet("errores/estadisticas")]
        public async Task<IActionResult> GetEstadisticasErrores()
        {
            try
            {
                var errores = await _unitOfWork.ErrorLogs.GetAllAsync();
                var lista = errores.ToList();

                var estadisticas = new
                {
                    totalErrores = lista.Count,
                    erroresNoRevisados = lista.Count(e => !e.Revisado),
                    erroresHoy = lista.Count(e => e.Fecha.Date == DateTime.UtcNow.Date),
                    erroresSemana = lista.Count(e => e.Fecha >= DateTime.UtcNow.AddDays(-7)),
                    erroresMes = lista.Count(e => e.Fecha >= DateTime.UtcNow.AddMonths(-1)),
                    erroresPorNivel = lista
                        .GroupBy(e => e.Nivel)
                        .Select(g => new { nivel = g.Key, cantidad = g.Count() })
                        .OrderByDescending(x => x.cantidad),
                    erroresPorTipo = lista
                        .GroupBy(e => e.TipoError)
                        .Select(g => new { tipo = g.Key, cantidad = g.Count() })
                        .OrderByDescending(x => x.cantidad)
                        .Take(5),
                    ultimosErrores = lista
                        .OrderByDescending(e => e.Fecha)
                        .Take(10)
                        .Select(e => new
                        {
                            e.TipoError,
                            e.Mensaje,
                            e.Nivel,
                            e.Fecha
                        })
                };

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Marca un error como revisado
        /// </summary>
        [HttpPut("errores/{id}/revisar")]
        public async Task<IActionResult> MarcarErrorRevisado(int id, [FromBody] string? notas = null)
        {
            try
            {
                var error = await _unitOfWork.ErrorLogs.GetByIdAsync(id);
                if (error == null)
                    return NotFound(new { mensaje = "Error no encontrado" });

                error.Revisado = true;
                error.Notas = notas;

                await _unitOfWork.ErrorLogs.UpdateAsync(error);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Error marcado como revisado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Genera reporte PDF de errores
        /// </summary>
        [HttpGet("errores/pdf")]
        public async Task<IActionResult> GenerarPDFErrores([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var errores = await _unitOfWork.ErrorLogs.GetAllAsync();
                var query = errores.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(e => e.Fecha >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(e => e.Fecha <= fechaFin.Value);

                var lista = query.OrderByDescending(e => e.Fecha).ToList();
                var html = GenerarHTMLErrores(lista);

                return Ok(new
                {
                    html = html,
                    nombreArchivo = $"Errores_Sistema_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        #endregion

        #region Helpers

        private string GenerarHTMLIntentosLogin(List<Domain.Entities.IntentosLogin> intentos)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Reporte de Intentos de Login</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 20px; }
                    h1 { color: #2c3e50; text-align: center; }
                    .info { text-align: center; color: #7f8c8d; margin-bottom: 20px; }
                    table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 12px; }
                    th { background-color: #6366F1; color: white; padding: 10px; text-align: left; }
                    td { padding: 8px; border-bottom: 1px solid #ddd; }
                    tr:nth-child(even) { background-color: #f9f9f9; }
                    .exitoso { color: #27ae60; font-weight: bold; }
                    .fallido { color: #e74c3c; font-weight: bold; }
                    .resumen { background: #f5f5f5; padding: 15px; border-radius: 8px; margin-bottom: 20px; }
                    .resumen-item { display: inline-block; margin-right: 30px; }
                </style>
            </head>
            <body>
                <h1>🔐 Reporte de Intentos de Login</h1>
                <p class='info'>Sistema de Gestión - Auditoría de Accesos</p>
                <p class='info'>Generado el: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</p>
                
                <div class='resumen'>
                    <span class='resumen-item'><strong>Total intentos:</strong> " + intentos.Count + @"</span>
                    <span class='resumen-item'><strong>Exitosos:</strong> " + intentos.Count(i => i.Exitoso) + @"</span>
                    <span class='resumen-item'><strong>Fallidos:</strong> " + intentos.Count(i => !i.Exitoso) + @"</span>
                </div>
                
                <table>
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Usuario</th>
                            <th>Estado</th>
                            <th>IP</th>
                            <th>Navegador</th>
                            <th>Mensaje</th>
                        </tr>
                    </thead>
                    <tbody>");

            foreach (var intento in intentos)
            {
                var estado = intento.Exitoso ? "<span class='exitoso'>✓ Exitoso</span>" : "<span class='fallido'>✗ Fallido</span>";
                sb.AppendLine($@"
                        <tr>
                            <td>{intento.FechaIntento:dd/MM/yyyy HH:mm:ss}</td>
                            <td>{intento.NombreUsuario}</td>
                            <td>{estado}</td>
                            <td>{intento.IpAddress ?? "-"}</td>
                            <td>{(intento.UserAgent?.Length > 50 ? intento.UserAgent.Substring(0, 50) + "..." : intento.UserAgent ?? "-")}</td>
                            <td>{intento.MensajeError ?? "-"}</td>
                        </tr>");
            }

            sb.AppendLine($@"
                    </tbody>
                </table>
                
                <div class='info' style='margin-top: 30px;'>
                    <p>Sistema de Gestión © {DateTime.Now.Year}</p>
                </div>
            </body>
            </html>");

            return sb.ToString();
        }

        private string GenerarHTMLErrores(List<Domain.Entities.ErrorLog> errores)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Reporte de Errores del Sistema</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 20px; }
                    h1 { color: #2c3e50; text-align: center; }
                    .info { text-align: center; color: #7f8c8d; margin-bottom: 20px; }
                    table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 12px; }
                    th { background-color: #6366F1; color: white; padding: 10px; text-align: left; }
                    td { padding: 8px; border-bottom: 1px solid #ddd; }
                    tr:nth-child(even) { background-color: #f9f9f9; }
                    .critical { color: #c0392b; font-weight: bold; }
                    .error { color: #e74c3c; }
                    .warning { color: #f39c12; }
                    .info-nivel { color: #3498db; }
                    .resumen { background: #f5f5f5; padding: 15px; border-radius: 8px; margin-bottom: 20px; }
                    .resumen-item { display: inline-block; margin-right: 30px; }
                </style>
            </head>
            <body>
                <h1>⚠️ Reporte de Errores del Sistema</h1>
                <p class='info'>Sistema de Gestión - Registro de Errores</p>
                <p class='info'>Generado el: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</p>
                
                <div class='resumen'>
                    <span class='resumen-item'><strong>Total errores:</strong> " + errores.Count + @"</span>
                    <span class='resumen-item'><strong>Críticos:</strong> " + errores.Count(e => e.Nivel == "Critical") + @"</span>
                    <span class='resumen-item'><strong>Errores:</strong> " + errores.Count(e => e.Nivel == "Error") + @"</span>
                    <span class='resumen-item'><strong>Advertencias:</strong> " + errores.Count(e => e.Nivel == "Warning") + @"</span>
                </div>
                
                <table>
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Nivel</th>
                            <th>Tipo</th>
                            <th>Mensaje</th>
                            <th>Origen</th>
                            <th>Usuario</th>
                        </tr>
                    </thead>
                    <tbody>");

            foreach (var error in errores)
            {
                var nivelClass = error.Nivel?.ToLower() switch
                {
                    "critical" => "critical",
                    "error" => "error",
                    "warning" => "warning",
                    "info" => "info-nivel",
                    _ => ""
                };

                sb.AppendLine($@"
                        <tr>
                            <td>{error.Fecha:dd/MM/yyyy HH:mm:ss}</td>
                            <td class='{nivelClass}'>{error.Nivel}</td>
                            <td>{error.TipoError}</td>
                            <td>{error.Mensaje}</td>
                            <td>{error.Origen ?? "-"}</td>
                            <td>{error.UsuarioId ?? "-"}</td>
                        </tr>");
            }

            sb.AppendLine($@"
                    </tbody>
                </table>
                
                <div class='info' style='margin-top: 30px;'>
                    <p>Sistema de Gestión © {DateTime.Now.Year}</p>
                </div>
            </body>
            </html>");

            return sb.ToString();
        }

        #endregion
    }
}
