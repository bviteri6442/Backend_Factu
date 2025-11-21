using PuntoVenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for ErrorLog entity
    /// </summary>
    public interface IErrorLogRepository : IGenericRepository<ErrorLog>
    {
        Task<IEnumerable<ErrorLog>> GetErroresNoRevisadosAsync();
        Task<IEnumerable<ErrorLog>> GetErroresPorFechaAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<ErrorLog>> GetErroresPorUsuarioAsync(string usuarioId);
    }
}