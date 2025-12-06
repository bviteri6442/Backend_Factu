namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Role entity for RBAC (Role-Based Access Control)
    /// </summary>
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public List<string> Permisos { get; set; } = new();
    }
}
