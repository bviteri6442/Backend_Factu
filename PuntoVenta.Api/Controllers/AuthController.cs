using MediatR;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Features.Usuarios.Commands;
using PuntoVenta.Application.Features.Usuarios.Queries;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PuntoVenta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
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
                Email = loginDto.Email,
                Contrasena = loginDto.Contrasena
            };

            var result = await _mediator.Send(query);

            if (!result.Exitoso)
            {
                // Return 400 (BadRequest) for login failures instead of 401
                // This prevents the frontend from showing "Session Expired"
                return BadRequest(new
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
                    NombreUsuario = createUsuarioDto.NombreUsuario,
                    Email = createUsuarioDto.Email,
                    Nombre = createUsuarioDto.Nombre,
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

        /// <summary>
        /// Setup inicial - Crea el rol Administrador y el primer usuario admin
        /// Solo funciona si no hay usuarios en el sistema
        /// </summary>
        [HttpPost("setup")]
        public async Task<IActionResult> Setup([FromBody] SetupInicialDto setupDto)
        {
            try
            {
                // Verificar si ya existen usuarios
                var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
                if (usuarios.Any())
                {
                    return BadRequest(new
                    {
                        exitoso = false,
                        mensaje = "El sistema ya tiene usuarios configurados. No se puede ejecutar el setup inicial."
                    });
                }

                // Crear rol Administrador si no existe
                var roles = await _unitOfWork.Roles.GetAllAsync();
                Rol rolAdmin;
                
                if (!roles.Any(r => r.Nombre == "Administrador"))
                {
                    rolAdmin = new Rol
                    {
                        Nombre = "Administrador",
                        Descripcion = "Administrador del sistema con acceso total",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        Permisos = new List<string> { "todos" }
                    };
                    await _unitOfWork.Roles.AddAsync(rolAdmin);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    rolAdmin = roles.First(r => r.Nombre == "Administrador");
                }

                // Crear usuario administrador
                var command = new CreateUsuarioCommand
                {
                    NombreUsuario = setupDto.NombreUsuario ?? "admin",
                    Email = setupDto.Email,
                    Nombre = setupDto.Nombre ?? "Administrador",
                    Contrasena = setupDto.Contrasena,
                    RolId = rolAdmin.Id
                };

                await _mediator.Send(command);

                return Ok(new
                {
                    exitoso = true,
                    mensaje = "Setup inicial completado. Usuario administrador creado exitosamente.",
                    usuario = new
                    {
                        email = setupDto.Email,
                        nombreUsuario = setupDto.NombreUsuario ?? "admin",
                        rol = "Administrador"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    exitoso = false,
                    mensaje = $"Error en el setup inicial: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Crear roles adicionales (solo funciona después del setup inicial)
        /// </summary>
        [HttpPost("crear-rol")]
        public async Task<IActionResult> CrearRol([FromBody] CrearRolDto rolDto)
        {
            try
            {
                // Verificar si existe el rol
                var roles = await _unitOfWork.Roles.GetAllAsync();
                if (roles.Any(r => r.Nombre.Equals(rolDto.Nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest(new
                    {
                        exitoso = false,
                        mensaje = $"El rol '{rolDto.Nombre}' ya existe."
                    });
                }

                var nuevoRol = new Rol
                {
                    Nombre = rolDto.Nombre,
                    Descripcion = rolDto.Descripcion ?? $"Rol de {rolDto.Nombre}",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow,
                    Permisos = rolDto.Permisos ?? new List<string>()
                };

                await _unitOfWork.Roles.AddAsync(nuevoRol);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new
                {
                    exitoso = true,
                    mensaje = $"Rol '{rolDto.Nombre}' creado exitosamente.",
                    rol = new
                    {
                        id = nuevoRol.Id,
                        nombre = nuevoRol.Nombre,
                        descripcion = nuevoRol.Descripcion
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    exitoso = false,
                    mensaje = $"Error al crear rol: {ex.Message}"
                });
            }
        }
    }
}

/// <summary>
/// DTO para el setup inicial del sistema
/// </summary>
public class SetupInicialDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Contrasena { get; set; } = string.Empty;

    public string? NombreUsuario { get; set; }
    public string? Nombre { get; set; }
}

/// <summary>
/// DTO para crear un nuevo rol
/// </summary>
public class CrearRolDto
{
    [Required(ErrorMessage = "El nombre del rol es requerido")]
    public string Nombre { get; set; } = string.Empty;
    
    public string? Descripcion { get; set; }
    public List<string>? Permisos { get; set; }
}

