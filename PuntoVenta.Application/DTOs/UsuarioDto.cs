using System.ComponentModel.DataAnnotations;

namespace PuntoVenta.Application.DTOs
{
    /// <summary>
    /// DTO para Login de usuarios
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 10 caracteres")]
        public string? Contrasena { get; set; }
    }

    /// <summary>
    /// DTO para crear un Usuario
    /// </summary>
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres")]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 10 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
            ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial")]
        public string? Contrasena { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        public int RolId { get; set; }
    }

    /// <summary>
    /// DTO para actualizar un Usuario
    /// </summary>
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres")]
        public string? Nombre { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        public int? RolId { get; set; }

        public bool? Activo { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de Usuario
    /// </summary>
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        public string? Nombre { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaBloqueo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimoLogin { get; set; }
        public string? RolNombre { get; set; }
        public int RolId { get; set; }
    }

    /// <summary>
    /// DTO flexible para crear usuario desde el frontend
    /// Acepta tanto los nombres del frontend (cedula, nombreCompleto, correo) 
    /// como los del backend (nombreUsuario, nombre, email)
    /// </summary>
    public class CreateUsuarioRequestDto
    {
        // Campos del backend
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        public string? Nombre { get; set; }

        // Campos del frontend (aliases)
        public string? Cedula { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Contrasena { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        public int RolId { get; set; }
    }
}
