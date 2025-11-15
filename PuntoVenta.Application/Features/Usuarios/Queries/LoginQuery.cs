using MediatR;

namespace PuntoVenta.Application.Features.Usuarios.Queries
{
    public class LoginQuery : IRequest<AuthResponse>
    {
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
    }

    public class AuthResponse
    {
        public bool Exitoso { get; set; }
        public string? Token { get; set; }
        public string? UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Rol { get; set; }
        public string? Mensaje { get; set; }
    }
}
