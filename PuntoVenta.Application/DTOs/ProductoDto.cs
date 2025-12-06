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

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 50 caracteres")]
        public string? Codigo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock inicial es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public int StockMinimo { get; set; } = 10;
    }

    /// <summary>
    /// DTO para actualizar un Producto
    /// </summary>
    public class UpdateProductoDto
    {
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
        public string? Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0")]
        public decimal? PrecioCompra { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal? Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int? Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
        public int? StockMinimo { get; set; }

        public bool? Activo { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de Producto
    /// </summary>
    public class ProductoResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
