namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad de Usuario extendida de IdentityUser para el sistema de puntos de venta
    /// </summary>
    public class Usuario
    {
    public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// Número de cédula del usuario (único)
        /// </summary>
    public string Cedula { get; set; } = string.Empty;
        
        /// <summary>
        /// Correo electrónico (usado para login)
        /// </summary>
    public string Correo { get; set; } = string.Empty;
        
        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;
        
        /// <summary>
        /// Contraseña hasheada
        /// </summary>
    public string Contrasena { get; set; } = string.Empty;
        
        /// <summary>
        /// Indicador si el usuario está activo
        /// </summary>
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Fecha de bloqueo (NULL si no está bloqueado)
        /// </summary>
        public DateTime? FechaBloqueo { get; set; }
        
        /// <summary>
        /// Razón del bloqueo
        /// </summary>
    public string RazonBloqueo { get; set; } = string.Empty;
        
        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Fecha del último login
        /// </summary>
        public DateTime? FechaUltimoLogin { get; set; }
        
        /// <summary>
        /// Relación con Rol
        /// </summary>
    public int RolId { get; set; }
    public Rol? Rol { get; set; }
    }
}
