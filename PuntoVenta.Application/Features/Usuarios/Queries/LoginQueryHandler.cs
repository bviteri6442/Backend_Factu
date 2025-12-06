using MediatR;
using Microsoft.IdentityModel.Tokens;
using PuntoVenta.Application.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace PuntoVenta.Application.Features.Usuarios.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse();

            // Validar que no estén vacíos
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Contrasena))
            {
                response.Exitoso = false;
                response.Mensaje = "El email y contraseña son requeridos";
                return response;
            }

            try
            {
                // Obtener usuario por email
                var usuario = await _unitOfWork.Usuarios.GetByCorreoAsync(request.Email);
                if (usuario == null)
                {
                    // Registrar intento fallido
                    await _unitOfWork.IntentosLogin.IncrementarIntentosAsync(request.Email, "", "");
                    await _unitOfWork.SaveChangesAsync();

                    response.Exitoso = false;
                    response.Mensaje = "Email o contraseña incorrectos";
                    return response;
                }

                // Verificar si el usuario está activo
                if (!usuario.Activo)
                {
                    response.Exitoso = false;
                    response.Mensaje = "La cuenta está inactiva. Contacte al administrador.";
                    return response;
                }

                // Verificar contraseña (BCrypt)
                bool esContrasenValida = false;
                if (!string.IsNullOrEmpty(usuario.PasswordHash))
                {
                    // Usar BCrypt.EnhancedVerify porque se creó con EnhancedHashPassword
                    esContrasenValida = BCrypt.Net.BCrypt.EnhancedVerify(request.Contrasena, usuario.PasswordHash, HashType.SHA384);
                }
                
                if (!esContrasenValida)
                {
                    // Registrar intento fallido
                    await _unitOfWork.IntentosLogin.IncrementarIntentosAsync(request.Email, "", "");
                    await _unitOfWork.SaveChangesAsync();

                    response.Exitoso = false;
                    response.Mensaje = "Email o contraseña incorrectos";
                    return response;
                }

                // Login exitoso: Reiniciar intentos fallidos
                await _unitOfWork.IntentosLogin.ReiniciarIntentosAsync(request.Email);

                // Actualizar última fecha de login
                usuario.FechaUltimoLogin = DateTime.UtcNow;
                await _unitOfWork.Usuarios.UpdateAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                // Generar JWT
                var token = GenerarToken(usuario);

                response.Exitoso = true;
                response.Token = token;
                response.UsuarioId = usuario.Id;
                response.NombreUsuario = usuario.Nombre;
                response.Rol = usuario.RolNombre;
                response.Mensaje = "Login exitoso";
            }
            catch (Exception ex)
            {
                response.Exitoso = false;
                response.Mensaje = $"Error en el login: {ex.Message}";
            }

            return response;
        }

        private string GenerarToken(Domain.Entities.Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                    new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
                    new Claim("NombreUsuario", usuario.NombreUsuario ?? ""),
                    new Claim(ClaimTypes.Role, usuario.RolNombre ?? "Usuario")
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

