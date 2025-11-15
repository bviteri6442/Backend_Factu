namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad para registrar todos los errores ocurridos en el sistema
    /// </summary>
    public class ErrorLog
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Tipo de excepción
        /// </summary>
    public string TipoError { get; set; } = string.Empty;
        
        /// <summary>
        /// Mensaje de error
        /// </summary>
    public string Mensaje { get; set; } = string.Empty;
        
        /// <summary>
        /// Stack trace del error
        /// </summary>
    public string StackTrace { get; set; } = string.Empty;
        
        /// <summary>
        /// Fuente del error (clase, método)
        /// </summary>
    public string Fuente { get; set; } = string.Empty;
        
        /// <summary>
        /// Número de línea donde ocurrió el error
        /// </summary>
        public int? NumeroLinea { get; set; }
        
        /// <summary>
        /// Pantalla o endpoint donde ocurrió el error
        /// </summary>
    public string Pantalla { get; set; } = string.Empty;
        
        /// <summary>
        /// Usuario que experimentó el error
        /// </summary>
    public string UsuarioId { get; set; } = string.Empty;
        
        /// <summary>
        /// Evento que generó el error (ej: "CrearVenta", "LoginUsuario")
        /// </summary>
    public string Evento { get; set; } = string.Empty;
        
        /// <summary>
        /// Datos adicionales contextuales
        /// </summary>
    public string DatosAdicionales { get; set; } = string.Empty;
        
        /// <summary>
        /// Nivel de severidad (Info, Warning, Error, Critical)
        /// </summary>
    public string NivelSeveridad { get; set; } = string.Empty;
        
        /// <summary>
        /// Fecha y hora del error
        /// </summary>
        public DateTime FechaError { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Indicador si el error fue revisado
        /// </summary>
        public bool Revisado { get; set; } = false;
        
        /// <summary>
        /// Notas de soporte técnico
        /// </summary>
    public string NotasSoporte { get; set; } = string.Empty;
    }
}
