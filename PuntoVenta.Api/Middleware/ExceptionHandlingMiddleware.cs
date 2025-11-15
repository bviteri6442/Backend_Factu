using Microsoft.AspNetCore.Http;
using PuntoVenta.Application.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PuntoVenta.Api.Middleware
{
    /// <summary>
    /// Middleware global para manejo de excepciones no capturadas
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, unitOfWork);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IUnitOfWork unitOfWork)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorId = Guid.NewGuid().ToString();

            try
            {
                // Registrar el error en la base de datos
                var errorLog = new PuntoVenta.Domain.Entities.ErrorLog
                {
                    TipoError = exception.GetType().Name,
                    Mensaje = exception.Message,
                    StackTrace = exception.StackTrace,
                    Fuente = exception.Source,
                    NumeroLinea = null,
                    Pantalla = context.Request.Path,
                    Evento = context.Request.Method,
                    NivelSeveridad = "Error",
                    FechaError = DateTime.UtcNow,
                    Revisado = false,
                    UsuarioId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                };

                unitOfWork.ErrorLogs.AddAsync(errorLog).GetAwaiter().GetResult();
                unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // Si falla al registrar, continuar de todas formas
            }

            var response = new
            {
                errorId,
                message = "Ha ocurrido un error interno en el servidor",
                detail = exception.Message,
                timestamp = DateTime.UtcNow
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
