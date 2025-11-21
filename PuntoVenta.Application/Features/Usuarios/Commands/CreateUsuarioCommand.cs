using MediatR;
using PuntoVenta.Application.DTOs;

namespace PuntoVenta.Application.Features.Usuarios.Commands
{
    public class CreateUsuarioCommand : IRequest<int>
    {
        public string? Cedula { get; set; }
        public string? Correo { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Contrasena { get; set; }
        public string RolId { get; set; } = string.Empty; // Changed from int to string for MongoDB ObjectId
    }
}
