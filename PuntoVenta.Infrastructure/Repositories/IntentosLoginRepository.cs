using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository for IntentosLogin entity
    /// </summary>
    public class IntentosLoginRepository : GenericRepository<IntentosLogin>, IIntentosLoginRepository
    {
        public IntentosLoginRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<IntentosLogin?> GetByCorreoAsync(string correo)
        {
            return await _context.IntentosLogin
                .FirstOrDefaultAsync(il => il.NombreUsuario == correo);
        }

        public async Task IncrementarIntentosAsync(string correo, string ip, string userAgent)
        {
            // Simply log the failed attempt
            var intento = new IntentosLogin
            {
                NombreUsuario = correo,
                Exitoso = false,
                IpAddress = ip,
                UserAgent = userAgent,
                FechaIntento = DateTime.UtcNow,
                MensajeError = "Intento de login fallido"
            };
            
            await _context.IntentosLogin.AddAsync(intento);
        }

        public async Task ReiniciarIntentosAsync(string correo)
        {
            // Log successful login
            var intento = new IntentosLogin
            {
                NombreUsuario = correo,
                Exitoso = true,
                FechaIntento = DateTime.UtcNow,
                MensajeError = null
            };
            
            await _context.IntentosLogin.AddAsync(intento);
        }

        public async Task BloquearUsuarioAsync(string correo)
        {
            // This functionality is now handled differently
            // Could mark user as inactive in Usuario table if needed
            await Task.CompletedTask;
        }
    }
}
