namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad de Rol para control de acceso basado en roles (RBAC)
    /// </summary>
    public class Rol
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Nombre del rol (ej: "Administrador", "Usuario", "Vendedor")
        /// </summary>
    public string Nombre { get; set; } = string.Empty;
        
        /// <summary>
        /// Descripción del rol
        /// </summary>
    public string Descripcion { get; set; } = string.Empty;
        
        /// <summary>
        /// Indicador si el rol está activo
        /// </summary>
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Relación uno a muchos con Usuarios
        /// </summary>
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
