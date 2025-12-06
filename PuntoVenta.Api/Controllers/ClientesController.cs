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
    /// Controlador para gestionar clientes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ClienteResponseDto>>> GetClientes()
        {
            try
            {
                var clientes = await _unitOfWork.Clientes.GetAllAsync();
                var result = clientes
                    .Select(c => new ClienteResponseDto
                    {
                        Id = c.Id, // string (MongoDB ObjectId)
                        Nombre = c.Nombre,
                        Documento = c.Documento,
                        Email = c.Email,
                        Telefono = c.Telefono,
                        Direccion = c.Direccion,
                        Activo = c.Activo,
                        FechaCreacion = c.FechaCreacion
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un cliente específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> GetClienteById(int id)
        {
            try
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                var result = new ClienteResponseDto
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Documento = cliente.Documento,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono,
                    Direccion = cliente.Direccion,
                    Activo = cliente.Activo,
                    FechaCreacion = cliente.FechaCreacion
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ClienteResponseDto>> CreateCliente([FromBody] CreateClienteDto createClienteDto)
        {
            try
            {
                var cliente = new PuntoVenta.Domain.Entities.Cliente
                {
                    Nombre = createClienteDto.Nombre ?? string.Empty,
                    Documento = createClienteDto.Documento ?? string.Empty,
                    Email = createClienteDto.Email ?? string.Empty,
                    Telefono = createClienteDto.Telefono ?? string.Empty,
                    Direccion = createClienteDto.Direccion ?? string.Empty,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                await _unitOfWork.Clientes.AddAsync(cliente);
                await _unitOfWork.SaveChangesAsync();

                var result = new ClienteResponseDto
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Documento = cliente.Documento,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono,
                    Direccion = cliente.Direccion,
                    Activo = cliente.Activo,
                    FechaCreacion = cliente.FechaCreacion
                };

                return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ClienteResponseDto>> UpdateCliente(int id, [FromBody] UpdateClienteDto updateClienteDto)
        {
            try
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                cliente.Nombre = updateClienteDto.Nombre ?? cliente.Nombre;
                cliente.Email = updateClienteDto.Email ?? cliente.Email;
                cliente.Telefono = updateClienteDto.Telefono ?? cliente.Telefono;
                cliente.Direccion = updateClienteDto.Direccion ?? cliente.Direccion;

                if (updateClienteDto.Activo.HasValue)
                {
                    cliente.Activo = updateClienteDto.Activo.Value;
                }

                await _unitOfWork.Clientes.UpdateAsync(cliente);
                await _unitOfWork.SaveChangesAsync();

                var result = new ClienteResponseDto
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Documento = cliente.Documento,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono,
                    Direccion = cliente.Direccion,
                    Activo = cliente.Activo,
                    FechaCreacion = cliente.FechaCreacion
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Busca clientes por término (nombre, documento, correo)
        /// </summary>
        [HttpGet("buscar/{termino}")]
        public async Task<ActionResult<List<ClienteResponseDto>>> BuscarClientes(string termino)
        {
            try
            {
                var clientes = await _unitOfWork.Clientes.SearchAsync(termino);
                var result = clientes
                    .Select(c => new ClienteResponseDto
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Documento = c.Documento,
                        Email = c.Email,
                        Telefono = c.Telefono,
                        Direccion = c.Direccion,
                        Activo = c.Activo,
                        FechaCreacion = c.FechaCreacion
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Desactiva un cliente
        /// </summary>
        [HttpPut("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<bool>> DesactivarCliente(int id)
        {
            try
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                cliente.Activo = false;
                await _unitOfWork.Clientes.UpdateAsync(cliente);
                await _unitOfWork.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
