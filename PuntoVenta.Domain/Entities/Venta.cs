using System;
using System.Collections.Generic;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una orden de venta/factura
    /// </summary>
    public class Venta
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Número de factura único
        /// </summary>
    public string NumeroFactura { get; set; } = string.Empty;
        
        /// <summary>
        /// Fecha y hora de la venta
        /// </summary>
        public DateTime FechaVenta { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// ID del usuario que realizó la venta
        /// </summary>
    public string UsuarioId { get; set; } = string.Empty;
        
        /// <summary>
        /// Cliente asociado a la venta (opcional)
        /// </summary>
        public int? ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
        
        /// <summary>
        /// Subtotal de la venta (antes de impuestos)
        /// </summary>
        public decimal Subtotal { get; set; }
        
        /// <summary>
        /// Porcentaje de IVA aplicado
        /// </summary>
        public decimal PorcentajeIVA { get; set; } = 19m;
        
        /// <summary>
        /// Total de impuestos
        /// </summary>
        public decimal TotalImpuesto { get; set; }
        
        /// <summary>
        /// Total de la venta (subtotal + impuestos)
        /// </summary>
        public decimal TotalVenta { get; set; }
        
        /// <summary>
        /// Estado de la venta (Pendiente, Completada, Cancelada)
        /// </summary>
    public string Estado { get; set; } = "Completada";
        
        /// <summary>
        /// Detalles de la venta
        /// </summary>
        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        
        /// <summary>
        /// Observaciones de la venta
        /// </summary>
    public string Observaciones { get; set; } = string.Empty;
    }
}
