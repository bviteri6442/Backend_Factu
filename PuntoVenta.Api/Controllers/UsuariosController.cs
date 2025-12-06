using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Features.Usuarios.Commands;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System.Security.Claims;

namespace PuntoVenta.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UsuariosController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener todos los usuarios (solo administrador)
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
                var usuariosDto = usuarios.Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Nombre = u.Nombre,
                    Activo = u.Activo,
                    FechaBloqueo = u.FechaBloqueo,
                    FechaCreacion = u.FechaCreacion,
                    FechaUltimoLogin = u.FechaUltimoLogin,
                    RolId = u.RolId,
                    RolNombre = u.RolNombre
                }).ToList();

                return Ok(usuariosDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtener usuario por ID (solo administrador)
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                var usuarioDto = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    NombreUsuario = usuario.NombreUsuario,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Activo = usuario.Activo,
                    FechaBloqueo = usuario.FechaBloqueo,
                    FechaCreacion = usuario.FechaCreacion,
                    FechaUltimoLogin = usuario.FechaUltimoLogin,
                    RolId = usuario.RolId,
                    RolNombre = usuario.RolNombre
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Crear nuevo usuario (solo administrador)
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioRequestDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Verificar que el rol existe
                var rol = await _unitOfWork.Roles.GetByIdAsync(createDto.RolId);
                if (rol == null)
                {
                    return BadRequest(new { mensaje = "El rol especificado no existe" });
                }

                var command = new CreateUsuarioCommand
                {
                    NombreUsuario = createDto.NombreUsuario ?? createDto.Cedula ?? "",
                    Email = createDto.Email ?? createDto.Correo ?? "",
                    Nombre = createDto.Nombre ?? createDto.NombreCompleto ?? "",
                    Contrasena = createDto.Contrasena ?? "",
                    RolId = createDto.RolId
                };

                var resultado = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetUsuario), new { id = resultado }, new
                {
                    exitoso = true,
                    mensaje = "Usuario creado exitosamente",
                    usuarioId = resultado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar usuario (solo administrador)
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UpdateUsuarioDto updateUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                if (!string.IsNullOrEmpty(updateUsuarioDto.Nombre))
                    usuario.Nombre = updateUsuarioDto.Nombre;

                if (!string.IsNullOrEmpty(updateUsuarioDto.Email))
                    usuario.Email = updateUsuarioDto.Email;

                if (updateUsuarioDto.RolId.HasValue)
                {
                    var rol = await _unitOfWork.Roles.GetByIdAsync(updateUsuarioDto.RolId.Value);
                    if (rol == null)
                        return BadRequest(new { mensaje = "Rol no válido" });
                    
                    usuario.RolId = updateUsuarioDto.RolId.Value;
                    usuario.RolNombre = rol.Nombre;
                }

                if (updateUsuarioDto.Activo.HasValue)
                    usuario.Activo = updateUsuarioDto.Activo.Value;

                await _unitOfWork.Usuarios.UpdateAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Desactivar usuario (soft delete) y registrar la eliminación
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id, [FromQuery] string? motivo = null)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                // Obtener información del administrador que realiza la eliminación
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var adminNombre = User.FindFirst(ClaimTypes.Name)?.Value ?? "Administrador";

                // Registrar la eliminación
                var eliminacion = new EliminacionUsuario
                {
                    UsuarioEliminadoId = usuario.Id,
                    CedulaUsuarioEliminado = usuario.NombreUsuario,
                    NombreUsuarioEliminado = usuario.Nombre,
                    EmailUsuarioEliminado = usuario.Email,
                    RolUsuarioEliminado = usuario.RolNombre,
                    AdministradorId = int.TryParse(adminId, out int aid) ? aid : 0,
                    NombreAdministrador = adminNombre,
                    FechaEliminacion = DateTime.UtcNow,
                    MotivoEliminacion = motivo,
                    TipoEliminacion = "Desactivación",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                await _unitOfWork.EliminacionesUsuarios.AddAsync(eliminacion);

                // Desactivar el usuario
                usuario.Activo = false;
                await _unitOfWork.Usuarios.UpdateAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario desactivado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Desbloquear usuario bloqueado por intentos fallidos
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPost("{id}/desbloquear")]
        public async Task<IActionResult> DesbloquearUsuario(int id)
        {
            try
            {
                var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                usuario.FechaBloqueo = null;
                usuario.RazonBloqueo = string.Empty;
                await _unitOfWork.Usuarios.UpdateAsync(usuario);

                // Reiniciar intentos de login
                if (!string.IsNullOrEmpty(usuario.Email))
                {
                    await _unitOfWork.IntentosLogin.ReiniciarIntentosAsync(usuario.Email);
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario desbloqueado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Buscar usuarios por término
        /// </summary>
        [HttpGet("buscar/{termino}")]
        public async Task<IActionResult> BuscarUsuarios(string termino)
        {
            try
            {
                var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
                var resultado = usuarios.Where(u =>
                    (!string.IsNullOrEmpty(u.Nombre) && u.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.NombreUsuario) && u.NombreUsuario.Contains(termino, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
