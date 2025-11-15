using System.ComponentModel.DataAnnotations;

namespace PuntoVenta.Application.DTOs
{
    /// <summary>
    /// DTO para crear detalle de venta
    /// </summary>
    public class CreateDetalleVentaDto
    {
        [Required(ErrorMessage = "El producto es requerido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal Descuento { get; set; } = 0;
    }

    /// <summary>
    /// DTO para respuesta de detalle de venta
    /// </summary>
    public class DetalleVentaResponseDto
    {
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public string? ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>
    /// DTO para crear una Venta
    /// </summary>
    public class CreateVentaDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "El cliente es inválido")]
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "Los detalles de la venta son requeridos")]
        [MinLength(1, ErrorMessage = "La venta debe tener al menos un producto")]
        public List<CreateDetalleVentaDto> Detalles { get; set; } = new List<CreateDetalleVentaDto>();

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de Venta
    /// </summary>
    public class VentaResponseDto
    {
        public int VentaId { get; set; }
        public string? NumeroFactura { get; set; }
        public DateTime FechaVenta { get; set; }
        public string? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int? ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeIVA { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalVenta { get; set; }
        public string? Estado { get; set; }
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para respuesta detallada de Venta (con detalles)
    /// </summary>
    public class VentaDetailResponseDto
    {
        public int VentaId { get; set; }
        public string? NumeroFactura { get; set; }
        public DateTime FechaVenta { get; set; }
        public string? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int? ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeIVA { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalVenta { get; set; }
        public string? Estado { get; set; }
        public string? Observaciones { get; set; }
        public List<DetalleVentaResponseDto>? Detalles { get; set; }
    }
}
