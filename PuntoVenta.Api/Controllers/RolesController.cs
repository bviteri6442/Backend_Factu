using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;

namespace PuntoVenta.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtener todos los roles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();
                var rolesDto = roles.Select(r => new RolResponseDto
                {
                    Id = r.Id, // string (MongoDB ObjectId)
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Activo = r.Activo,
                    FechaCreacion = r.FechaCreacion
                }).ToList();

                return Ok(rolesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtener rol por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRol(int id)
        {
            try
            {
                var rol = await _unitOfWork.Roles.GetByIdAsync(id);
                if (rol == null)
                {
                    return NotFound(new { mensaje = "Rol no encontrado" });
                }

                var rolDto = new RolResponseDto
                {
                    Id = rol.Id,
                    Nombre = rol.Nombre,
                    Descripcion = rol.Descripcion,
                    Activo = rol.Activo,
                    FechaCreacion = rol.FechaCreacion
                };

                return Ok(rolDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Crear nuevo rol
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRol([FromBody] CreateRolDto createRolDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rol = new Rol
                {
                    Nombre = createRolDto.Nombre ?? string.Empty,
                    Descripcion = createRolDto.Descripcion ?? string.Empty,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                await _unitOfWork.Roles.AddAsync(rol);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, new { mensaje = $"Rol {createRolDto.Nombre} creado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar rol
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] UpdateRolDto updateRolDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rol = await _unitOfWork.Roles.GetByIdAsync(id);
                if (rol == null)
                {
                    return NotFound(new { mensaje = "Rol no encontrado" });
                }

                if (!string.IsNullOrEmpty(updateRolDto.Nombre))
                    rol.Nombre = updateRolDto.Nombre;

                if (!string.IsNullOrEmpty(updateRolDto.Descripcion))
                    rol.Descripcion = updateRolDto.Descripcion;

                if (updateRolDto.Activo.HasValue)
                    rol.Activo = updateRolDto.Activo.Value;

                await _unitOfWork.Roles.UpdateAsync(rol);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Rol actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Desactivar rol
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                var rol = await _unitOfWork.Roles.GetByIdAsync(id);
                if (rol == null)
                {
                    return NotFound(new { mensaje = "Rol no encontrado" });
                }

                rol.Activo = false;
                await _unitOfWork.Roles.UpdateAsync(rol);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { mensaje = "Rol desactivado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
