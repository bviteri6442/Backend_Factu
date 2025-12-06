namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Product entity for PostgreSQL
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal PrecioCompra { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; } = 10;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}
