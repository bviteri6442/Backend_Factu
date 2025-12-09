using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System.Text;

namespace PuntoVenta.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class EliminacionesProductosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EliminacionesProductosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtener todas las eliminaciones de productos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var eliminaciones = await _unitOfWork.EliminacionesProductos.GetAllAsync();
                var ordenadas = eliminaciones.OrderByDescending(e => e.FechaEliminacion);
                return Ok(ordenadas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtener eliminación por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var eliminacion = await _unitOfWork.EliminacionesProductos.GetByIdAsync(id);
                if (eliminacion == null)
                {
                    return NotFound(new { mensaje = "Registro de eliminación no encontrado" });
                }
                return Ok(eliminacion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Buscar eliminaciones por término
        /// </summary>
        [HttpGet("buscar/{termino}")]
        public async Task<IActionResult> Buscar(string termino)
        {
            try
            {
                var eliminaciones = await _unitOfWork.EliminacionesProductos.BuscarAsync(termino);
                return Ok(eliminaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Generar PDF con el reporte de eliminaciones
        /// </summary>
        [HttpGet("pdf")]
        public async Task<IActionResult> GenerarPDF([FromQuery] string? termino = null)
        {
            try
            {
                IEnumerable<EliminacionProducto> eliminaciones;
                
                if (!string.IsNullOrEmpty(termino))
                {
                    eliminaciones = await _unitOfWork.EliminacionesProductos.BuscarAsync(termino);
                }
                else
                {
                    eliminaciones = await _unitOfWork.EliminacionesProductos.GetAllAsync();
                }

                var html = GenerarHTMLReporte(eliminaciones.OrderByDescending(e => e.FechaEliminacion).ToList());
                
                return Ok(new { 
                    html = html,
                    nombreArchivo = $"Reporte_Eliminaciones_Productos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtener estadísticas de eliminaciones
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<IActionResult> GetEstadisticas()
        {
            try
            {
                var eliminaciones = await _unitOfWork.EliminacionesProductos.GetAllAsync();
                var lista = eliminaciones.ToList();

                var estadisticas = new
                {
                    totalEliminaciones = lista.Count,
                    eliminacionesHoy = lista.Count(e => e.FechaEliminacion.Date == DateTime.UtcNow.Date),
                    eliminacionesSemana = lista.Count(e => e.FechaEliminacion >= DateTime.UtcNow.AddDays(-7)),
                    eliminacionesMes = lista.Count(e => e.FechaEliminacion >= DateTime.UtcNow.AddMonths(-1)),
                    valorTotalEliminado = lista.Sum(e => e.PrecioVentaProductoEliminado * e.StockProductoEliminado),
                    stockTotalEliminado = lista.Sum(e => e.StockProductoEliminado),
                    porTipo = lista.GroupBy(e => e.TipoEliminacion)
                        .Select(g => new { tipo = g.Key, cantidad = g.Count() }),
                    ultimasEliminaciones = lista.OrderByDescending(e => e.FechaEliminacion).Take(5).Select(e => new
                    {
                        e.NombreProductoEliminado,
                        e.FechaEliminacion,
                        e.NombreAdministrador
                    })
                };

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        private string GenerarHTMLReporte(List<EliminacionProducto> eliminaciones)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine(@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Reporte de Eliminaciones de Productos</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 20px; }
                    h1 { color: #2c3e50; text-align: center; }
                    .info { text-align: center; color: #7f8c8d; margin-bottom: 20px; }
                    table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 12px; }
                    th { background-color: #6366F1; color: white; padding: 10px; text-align: left; }
                    td { padding: 8px; border-bottom: 1px solid #ddd; }
                    tr:nth-child(even) { background-color: #f9f9f9; }
                    .footer { text-align: center; margin-top: 30px; color: #7f8c8d; font-size: 12px; }
                    .total { font-weight: bold; margin-top: 20px; }
                    .resumen { background: #f5f5f5; padding: 15px; border-radius: 8px; margin-bottom: 20px; }
                    .resumen-item { display: inline-block; margin-right: 30px; }
                </style>
            </head>
            <body>
                <h1>📦 Reporte de Eliminaciones de Productos</h1>
                <p class='info'>Sistema de Gestión - Punto de Venta</p>
                <p class='info'>Generado el: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</p>
                
                <div class='resumen'>
                    <span class='resumen-item'><strong>Total eliminados:</strong> " + eliminaciones.Count + @"</span>
                    <span class='resumen-item'><strong>Stock total eliminado:</strong> " + eliminaciones.Sum(e => e.StockProductoEliminado) + @" unidades</span>
                    <span class='resumen-item'><strong>Valor total:</strong> $" + eliminaciones.Sum(e => e.PrecioVentaProductoEliminado * e.StockProductoEliminado).ToString("N2") + @"</span>
                </div>
                
                <table>
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Código</th>
                            <th>Producto</th>
                            <th>P. Costo</th>
                            <th>P. Venta</th>
                            <th>Stock</th>
                            <th>Eliminado por</th>
                            <th>Tipo</th>
                            <th>Motivo</th>
                        </tr>
                    </thead>
                    <tbody>");

            foreach (var e in eliminaciones)
            {
                sb.AppendLine($@"
                        <tr>
                            <td>{e.FechaEliminacion:dd/MM/yyyy HH:mm}</td>
                            <td>{e.CodigoProductoEliminado}</td>
                            <td>{e.NombreProductoEliminado}</td>
                            <td>${e.PrecioCostoProductoEliminado:N2}</td>
                            <td>${e.PrecioVentaProductoEliminado:N2}</td>
                            <td>{e.StockProductoEliminado}</td>
                            <td>{e.NombreAdministrador}</td>
                            <td>{e.TipoEliminacion}</td>
                            <td>{e.MotivoEliminacion ?? "-"}</td>
                        </tr>");
            }

            sb.AppendLine($@"
                    </tbody>
                </table>
                
                <p class='total'>Total de registros: {eliminaciones.Count}</p>
                
                <div class='footer'>
                    <p>Sistema de Gestión - Punto de Venta © {DateTime.Now.Year}</p>
                </div>
            </body>
            </html>");

            return sb.ToString();
        }
    }
}
