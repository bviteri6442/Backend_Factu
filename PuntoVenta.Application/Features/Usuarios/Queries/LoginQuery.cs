using MediatR;

namespace PuntoVenta.Application.Features.Usuarios.Queries
{
    public class LoginQuery : IRequest<AuthResponse>
    {
        public string? Email { get; set; }
        public string? Contrasena { get; set; }
    }

    public class AuthResponse
    {
        public bool Exitoso { get; set; }
        public string? Token { get; set; }
        public int UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Rol { get; set; }
        public string? Mensaje { get; set; }
    }
}
