using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Generic repository interface for PostgreSQL entities with EF Core
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}