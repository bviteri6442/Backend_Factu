using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;

namespace PuntoVenta.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de logging centralizado
    /// Siguiendo el principio de Single Responsibility: solo se encarga de registrar logs
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoggingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogErrorAsync(string mensaje, Exception? excepcion = null, string? usuarioId = null, string? origen = null)
        {
            var errorLog = new ErrorLog
            {
                TipoError = excepcion?.GetType().Name ?? "Error",
                Mensaje = mensaje,
                StackTrace = excepcion?.StackTrace,
                Origen = origen ?? excepcion?.Source,
                UsuarioId = usuarioId,
                Nivel = "Error",
                Fecha = DateTime.UtcNow,
                Revisado = false
            };

            await _unitOfWork.ErrorLogs.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LogErrorAsync(string tipoError, string mensaje, string? stackTrace = null, string? origen = null, int? numeroLinea = null, string? usuarioId = null, string nivel = "Error")
        {
            var errorLog = new ErrorLog
            {
                TipoError = tipoError,
                Mensaje = mensaje,
                StackTrace = stackTrace,
                Origen = origen,
                NumeroLinea = numeroLinea,
                UsuarioId = usuarioId,
                Nivel = nivel,
                Fecha = DateTime.UtcNow,
                Revisado = false
            };

            await _unitOfWork.ErrorLogs.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LogInfoAsync(string mensaje, string? usuarioId = null)
        {
            var errorLog = new ErrorLog
            {
                TipoError = "Info",
                Mensaje = mensaje,
                UsuarioId = usuarioId,
                Nivel = "Info",
                Fecha = DateTime.UtcNow,
                Revisado = true // Los logs de info se marcan como revisados automáticamente
            };

            await _unitOfWork.ErrorLogs.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LogWarningAsync(string mensaje, string? usuarioId = null)
        {
            var errorLog = new ErrorLog
            {
                TipoError = "Warning",
                Mensaje = mensaje,
                UsuarioId = usuarioId,
                Nivel = "Warning",
                Fecha = DateTime.UtcNow,
                Revisado = false
            };

            await _unitOfWork.ErrorLogs.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LogCriticalAsync(string mensaje, Exception? excepcion = null, string? usuarioId = null)
        {
            var errorLog = new ErrorLog
            {
                TipoError = excepcion?.GetType().Name ?? "Critical",
                Mensaje = mensaje,
                StackTrace = excepcion?.StackTrace,
                Origen = excepcion?.Source,
                UsuarioId = usuarioId,
                Nivel = "Critical",
                Fecha = DateTime.UtcNow,
                Revisado = false
            };

            await _unitOfWork.ErrorLogs.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
