using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository for ErrorLog entity
    /// </summary>
    public class ErrorLogRepository : GenericRepository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLogRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<ErrorLog>> GetErroresNoRevisadosAsync()
        {
            return await _context.ErrorLogs
                .Where(e => !e.Revisado)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErroresPorFechaAsync(DateTime desde, DateTime hasta)
        {
            return await _context.ErrorLogs
                .Where(e => e.Fecha >= desde && e.Fecha <= hasta)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<ErrorLog>> GetErroresPorUsuarioAsync(string usuarioId)
        {
            return await _context.ErrorLogs
                .Where(e => e.UsuarioId == usuarioId)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }
    }
}
