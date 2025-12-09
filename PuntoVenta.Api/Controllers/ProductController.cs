using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.Features.Products.Commands.CreateProduct;
using PuntoVenta.Application.Features.Products.Commands.UpdateProduct;
using PuntoVenta.Application.Features.Products.Commands.DeleteProduct;
using PuntoVenta.Application.Features.Products.Queries.GetProducts;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System.Security.Claims;

namespace PuntoVenta.Api.Controllers
{
    [Authorize]
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ProductosController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _mediator.Send(new GetProductsQuery());
            return Ok(productos);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductos), new { id }, new { Message = $"Producto creado con ID: {id}" });
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest();
            
            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id, [FromQuery] string? motivo = null)
        {
            try
            {
                // Obtener el producto antes de eliminarlo
                var producto = await _unitOfWork.Productos.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                // Obtener información del administrador
                var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var adminNombre = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";

                // Registrar la eliminación
                var eliminacion = new EliminacionProducto
                {
                    ProductoEliminadoId = producto.Id,
                    CodigoProductoEliminado = producto.Codigo,
                    NombreProductoEliminado = producto.Nombre,
                    DescripcionProductoEliminado = producto.Descripcion,
                    PrecioVentaProductoEliminado = producto.Precio,
                    PrecioCostoProductoEliminado = producto.PrecioCompra,
                    StockProductoEliminado = producto.Stock,
                    AdministradorId = adminId,
                    NombreAdministrador = adminNombre,
                    FechaEliminacion = DateTime.UtcNow,
                    MotivoEliminacion = motivo,
                    TipoEliminacion = "Desactivación",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                await _unitOfWork.EliminacionesProductos.AddAsync(eliminacion);

                // Desactivar el producto
                producto.Activo = false;
                producto.FechaActualizacion = DateTime.UtcNow;
                await _unitOfWork.Productos.UpdateAsync(producto);

                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Producto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
