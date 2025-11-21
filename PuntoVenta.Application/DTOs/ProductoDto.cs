using System.ComponentModel.DataAnnotations;

namespace PuntoVenta.Application.DTOs
{
    /// <summary>
    /// DTO para crear un Producto
    /// </summary>
    public class CreateProductoDto
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El código de barras es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El código de barras debe tener entre 3 y 50 caracteres")]
        public string? CodigoBarra { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio de costo es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de costo debe ser mayor a 0")]
        public decimal PrecioCosto { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor a 0")]
        public decimal PrecioVenta { get; set; }

        [Required(ErrorMessage = "El stock inicial es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int StockActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public int StockMinimo { get; set; } = 10;
    }

    /// <summary>
    /// DTO para actualizar un Producto
    /// </summary>
    public class UpdateProductoDto
    {
        public string? Id { get; set; } // Changed from int to string for MongoDB ObjectId

        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
        public string? Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de costo debe ser mayor a 0")]
        public decimal? PrecioCosto { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor a 0")]
        public decimal? PrecioVenta { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int? StockActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public int? StockMinimo { get; set; }

        public bool? Activo { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de Producto
    /// </summary>
    public class ProductoResponseDto
    {
        public string Id { get; set; } = string.Empty; // Changed from int to string for MongoDB ObjectId
        public string? Nombre { get; set; }
        public string? CodigoBarra { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
