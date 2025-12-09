namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Servicio para registro centralizado de logs del sistema
    /// Siguiendo el principio de Single Responsibility
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Registra un error en el sistema
        /// </summary>
        Task LogErrorAsync(string mensaje, Exception? excepcion = null, string? usuarioId = null, string? origen = null);

        /// <summary>
        /// Registra un error con información detallada
        /// </summary>
        Task LogErrorAsync(string tipoError, string mensaje, string? stackTrace = null, string? origen = null, int? numeroLinea = null, string? usuarioId = null, string nivel = "Error");

        /// <summary>
        /// Registra información general del sistema
        /// </summary>
        Task LogInfoAsync(string mensaje, string? usuarioId = null);

        /// <summary>
        /// Registra una advertencia
        /// </summary>
        Task LogWarningAsync(string mensaje, string? usuarioId = null);

        /// <summary>
        /// Registra un evento crítico
        /// </summary>
        Task LogCriticalAsync(string mensaje, Exception? excepcion = null, string? usuarioId = null);
    }
}
