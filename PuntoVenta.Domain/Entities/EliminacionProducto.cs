using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad para registrar las eliminaciones de productos del sistema
    /// </summary>
    [Table("eliminaciones_productos")]
    public class EliminacionProducto
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ID del producto eliminado
        /// </summary>
        public int ProductoEliminadoId { get; set; }

        /// <summary>
        /// Código de barras del producto eliminado
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CodigoProductoEliminado { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del producto eliminado
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string NombreProductoEliminado { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del producto eliminado
        /// </summary>
        [MaxLength(500)]
        public string? DescripcionProductoEliminado { get; set; }

        /// <summary>
        /// Precio de venta del producto eliminado
        /// </summary>
        public decimal PrecioVentaProductoEliminado { get; set; }

        /// <summary>
        /// Precio de costo del producto eliminado
        /// </summary>
        public decimal PrecioCostoProductoEliminado { get; set; }

        /// <summary>
        /// Stock que tenía el producto al momento de eliminarse
        /// </summary>
        public int StockProductoEliminado { get; set; }

        /// <summary>
        /// ID del administrador que realizó la eliminación
        /// </summary>
        public int AdministradorId { get; set; }

        /// <summary>
        /// Nombre del administrador que realizó la eliminación
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string NombreAdministrador { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de la eliminación
        /// </summary>
        public DateTime FechaEliminacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Motivo de la eliminación (opcional)
        /// </summary>
        [MaxLength(500)]
        public string? MotivoEliminacion { get; set; }

        /// <summary>
        /// Tipo de eliminación: "Desactivación" o "Eliminación permanente"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string TipoEliminacion { get; set; } = "Desactivación";

        /// <summary>
        /// IP desde donde se realizó la eliminación
        /// </summary>
        [MaxLength(50)]
        public string? IpAddress { get; set; }
    }
}
