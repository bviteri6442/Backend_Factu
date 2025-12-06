using Microsoft.AspNetCore.Http;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using System;
using System.Net;
using System.Security.Claims;
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IUnitOfWork unitOfWork)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorId = Guid.NewGuid().ToString();

            try
            {
                // Registrar el error en la base de datos
                var errorLog = new ErrorLog
                {
                    TipoError = exception.GetType().Name,
                    Mensaje = exception.Message,
                    StackTrace = exception.StackTrace,
                    Origen = exception.Source,
                    NumeroLinea = null,
                    UsuarioId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Nivel = "Error",
                    Fecha = DateTime.UtcNow,
                    Revisado = false,
                    Notas = $"Ruta: {context.Request.Path}; Método: {context.Request.Method}"
                };

                await unitOfWork.ErrorLogs.AddAsync(errorLog);
                await unitOfWork.SaveChangesAsync();
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

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
