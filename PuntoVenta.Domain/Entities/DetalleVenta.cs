namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad que representa el detalle de una línea de venta
    /// </summary>
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? CodigoBarra { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; } = 0m;
        public decimal Total { get; set; }
    }
}
