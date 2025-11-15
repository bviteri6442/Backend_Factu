using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.Features.Products.Commands.CreateProduct;
using PuntoVenta.Application.Features.Products.Commands.UpdateProduct;
using PuntoVenta.Application.Features.Products.Commands.DeleteProduct;
using PuntoVenta.Application.Features.Products.Queries.GetProducts;

namespace PuntoVenta.Api.Controllers
{
    [Authorize]
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductosController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<IActionResult> EliminarProducto(int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return NoContent();
        }
    }
}
