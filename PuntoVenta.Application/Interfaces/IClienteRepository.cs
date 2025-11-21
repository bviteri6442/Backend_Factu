using PuntoVenta.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Cliente entity
    /// </summary>
    public interface IClienteRepository : IGenericRepository<Cliente>
    {
        Task<Cliente?> GetByDocumentoAsync(string documento);
        Task<IEnumerable<Cliente>> SearchAsync(string searchTerm);
    }
}