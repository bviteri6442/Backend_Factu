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
                // Validar que no exista usuario con el mismo email
                if (await _unitOfWork.Usuarios.ExisteCorreoAsync(request.Email ?? string.Empty))
                {
                    throw new Exception("Ya existe un usuario con este email");
                }

                // Validar que no exista usuario con el mismo nombre de usuario
                if (await _unitOfWork.Usuarios.ExisteCedulaAsync(request.NombreUsuario ?? string.Empty))
                {
                    throw new Exception("Ya existe un usuario con este nombre de usuario");
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
                    NombreUsuario = request.NombreUsuario ?? string.Empty,
                    Email = request.Email ?? string.Empty,
                    Nombre = request.Nombre ?? string.Empty,
                    PasswordHash = contraseniaHasheada,
                    RolId = request.RolId,
                    RolNombre = rol.Nombre,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                await _unitOfWork.Usuarios.AddAsync(nuevoUsuario);
                await _unitOfWork.SaveChangesAsync();

                return nuevoUsuario.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el usuario: {ex.Message}");
            }
        }
    }
}
