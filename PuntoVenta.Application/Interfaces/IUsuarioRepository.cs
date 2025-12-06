using PuntoVenta.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Usuario entity
    /// </summary>
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<int> AddAsync(Usuario entity);
        Task UpdateAsync(Usuario entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task<Usuario?> GetByCedulaAsync(string cedula);
        Task<IEnumerable<Usuario>> GetByRolAsync(int rolId);
        Task<bool> ExisteCorreoAsync(string correo);
        Task<bool> ExisteCedulaAsync(string cedula);
    }
}