using MediatR;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using BCrypt.Net;

namespace PuntoVenta.Application.Features.Usuarios.Commands
{
    public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUsuarioCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validar que no exista usuario con el mismo correo
                if (await _unitOfWork.Usuarios.ExisteCorreoAsync(request.Correo ?? string.Empty))
                {
                    throw new Exception("Ya existe un usuario con este correo");
                }

                // Validar que no exista usuario con la misma cédula
                if (await _unitOfWork.Usuarios.ExisteCedulaAsync(request.Cedula ?? string.Empty))
                {
                    throw new Exception("Ya existe un usuario con esta cédula");
                }

                // Verificar que el rol existe
                var rol = await _unitOfWork.Roles.GetByIdAsync(request.RolId);
                if (rol == null)
                {
                    throw new Exception("El rol especificado no existe");
                }

                // Hash de contraseña usando BCrypt
                var contraseniaHasheada = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Contrasena, HashType.SHA384);

                // Crear nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    Cedula = request.Cedula ?? string.Empty,
                    Correo = request.Correo ?? string.Empty,
                    NombreCompleto = request.NombreCompleto ?? string.Empty,
                    ContrasenaHash = contraseniaHasheada, // Cambiado de Contrasena a ContrasenaHash
                    RolId = request.RolId,
                    RolNombre = rol.Nombre, // Desnormalizar nombre del rol para MongoDB
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                await _unitOfWork.Usuarios.AddAsync(nuevoUsuario);
                await _unitOfWork.SaveChangesAsync();

                return 1; // Retornar 1 para indicar éxito
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el usuario: {ex.Message}");
            }
        }
    }
}
