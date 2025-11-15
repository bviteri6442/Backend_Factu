namespace PuntoVenta.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Nombre del cliente
        /// </summary>
    public string Nombre { get; set; } = string.Empty;
        
        /// <summary>
        /// Documento de identidad (cédula, pasaporte, etc.)
        /// </summary>
    public string Documento { get; set; } = string.Empty;
        
        /// <summary>
        /// Dirección del cliente
        /// </summary>
    public string Direccion { get; set; } = string.Empty;
        
        /// <summary>
        /// Teléfono de contacto
        /// </summary>
    public string Telefono { get; set; } = string.Empty;
        
        /// <summary>
        /// Correo electrónico del cliente
        /// </summary>
    public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Indicador si el cliente está activo
        /// </summary>
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Relación uno a muchos con Ventas
        /// </summary>
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
