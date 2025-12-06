namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Invoice (Factura) entity for PostgreSQL
    /// Represents a complete sale transaction with related details
    /// </summary>
    public class Factura
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime FechaVenta { get; set; } = DateTime.UtcNow;
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public int? ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string? ClienteDocumento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeIVA { get; set; } = 12m;
        public decimal TotalImpuesto { get; set; }
        public decimal TotalVenta { get; set; }
        public string Estado { get; set; } = "Completada";
        public string? Observaciones { get; set; }
        
        // Navigation property
        public List<DetalleVenta> Detalles { get; set; } = new();
    }
}