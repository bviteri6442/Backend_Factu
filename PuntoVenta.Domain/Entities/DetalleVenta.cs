namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad que representa el detalle de una línea de venta
    /// </summary>
    public class DetalleVenta
    {
        public int VentaId { get; set; }
    public Venta? Venta { get; set; }

        public int ProductoId { get; set; }
    public Product? Producto { get; set; }

        /// <summary>
        /// Cantidad de productos vendidos
        /// </summary>
        public int Cantidad { get; set; }
        
        /// <summary>
        /// Precio unitario en el momento de la venta
        /// </summary>
        public decimal PrecioUnitario { get; set; }
        
        /// <summary>
        /// Descuento aplicado al detalle (en porcentaje o valor)
        /// </summary>
        public decimal Descuento { get; set; } = 0;
        
        /// <summary>
        /// Total del detalle (Cantidad * PrecioUnitario - Descuento)
        /// </summary>
        public decimal Total { get; set; }
    }
}
