using MediatR;
using PuntoVenta.Application.DTOs;

namespace PuntoVenta.Application.Features.Usuarios.Commands
{
    public class CreateUsuarioCommand : IRequest<int>
    {
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        public string? Nombre { get; set; }
        public string? Contrasena { get; set; }
        public int RolId { get; set; }
    }
}
