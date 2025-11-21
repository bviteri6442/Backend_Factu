using PuntoVenta.Domain.Entities;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Rol entity
    /// </summary>
    public interface IRolRepository : IGenericRepository<Rol>
    {
        Task GetByIdAsync(int id);
        Task<Rol?> GetByNombreAsync(string nombre);
    }
}