using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad para registrar las eliminaciones de usuarios del sistema
    /// </summary>
    [Table("eliminaciones_usuarios")]
    public class EliminacionUsuario
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ID del usuario eliminado
        /// </summary>
        public int UsuarioEliminadoId { get; set; }

        /// <summary>
        /// Cédula/Nombre de usuario del usuario eliminado
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CedulaUsuarioEliminado { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario eliminado
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string NombreUsuarioEliminado { get; set; } = string.Empty;

        /// <summary>
        /// Email del usuario eliminado
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string EmailUsuarioEliminado { get; set; } = string.Empty;

        /// <summary>
        /// Rol que tenía el usuario eliminado
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string RolUsuarioEliminado { get; set; } = string.Empty;

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
