namespace PuntoVenta.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Nombre del producto
        /// </summary>
    public string Nombre { get; set; } = string.Empty;
        
        /// <summary>
        /// Código de barras único del producto
        /// </summary>
    public string CodigoBarra { get; set; } = string.Empty;
        
        /// <summary>
        /// Descripción del producto
        /// </summary>
    public string Descripcion { get; set; } = string.Empty;
        
        /// <summary>
        /// Precio de costo
        /// </summary>
        public decimal PrecioCosto { get; set; }
        
        /// <summary>
        /// Precio de venta
        /// </summary>
        public decimal PrecioVenta { get; set; }
        
        /// <summary>
        /// Stock actual del producto
        /// </summary>
        public int StockActual { get; set; }
        
        /// <summary>
        /// Stock mínimo antes de alertar
        /// </summary>
        public int StockMinimo { get; set; } = 10;
        
        /// <summary>
        /// Indicador si el producto está activo
        /// </summary>
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}
