namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad para rastrear intentos de login fallidos de usuarios
    /// Usado para implementar bloqueo automático después de 3 intentos fallidos
    /// </summary>
    public class IntentosLogin
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Email del usuario que intentó acceder
        /// </summary>
    public string Correo { get; set; } = string.Empty;
        
        /// <summary>
        /// Número de intentos fallidos consecutivos
        /// </summary>
        public int NumeroIntentosFallidos { get; set; } = 0;
        
        /// <summary>
        /// Fecha del último intento fallido
        /// </summary>
        public DateTime FechaUltimoIntento { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Dirección IP desde donde se intentó el acceso
        /// </summary>
    public string DireccionIP { get; set; } = string.Empty;
        
        /// <summary>
        /// User Agent del navegador
        /// </summary>
    public string UserAgent { get; set; } = string.Empty;
        
        /// <summary>
        /// Indicador si la cuenta fue bloqueada por intentos
        /// </summary>
        public bool Bloqueado { get; set; } = false;
        
        /// <summary>
        /// Fecha de bloqueo
        /// </summary>
        public DateTime? FechaBloqueo { get; set; }
    }
}
