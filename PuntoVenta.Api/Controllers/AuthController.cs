using MediatR;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Features.Usuarios.Commands;
using PuntoVenta.Application.Features.Usuarios.Queries;
using System.ComponentModel.DataAnnotations;

namespace PuntoVenta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Login de usuario con correo y contraseña
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = new LoginQuery
            {
                Correo = loginDto.Correo,
                Contrasena = loginDto.Contrasena
            };

            var result = await _mediator.Send(query);

            if (!result.Exitoso)
            {
                return Unauthorized(new
                {
                    exitoso = false,
                    mensaje = result.Mensaje
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Registro de nuevo usuario (solo por administrador)
        /// </summary>
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] CreateUsuarioDto createUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateUsuarioCommand
                {
                    Cedula = createUsuarioDto.Cedula,
                    Correo = createUsuarioDto.Correo,
                    NombreCompleto = createUsuarioDto.NombreCompleto,
                    Contrasena = createUsuarioDto.Contrasena,
                    RolId = createUsuarioDto.RolId
                };

                var resultado = await _mediator.Send(command);

                return CreatedAtAction(nameof(Login), new
                {
                    exitoso = true,
                    mensaje = "Usuario creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    exitoso = false,
                    mensaje = ex.Message
                });
            }
        }
    }
}

