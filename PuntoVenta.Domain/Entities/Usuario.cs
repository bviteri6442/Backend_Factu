namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// User entity for PostgreSQL with authentication
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // BCrypt hash
        public bool Activo { get; set; } = true;
        public DateTime? FechaBloqueo { get; set; }
        public string? RazonBloqueo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaUltimoLogin { get; set; }
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
    }
}
