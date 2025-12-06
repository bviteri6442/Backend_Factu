using PuntoVenta.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Product entity with custom queries
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByCodigoBarraAsync(string codigoBarra);
        Task<IEnumerable<Product>> GetProductosConStockAsync();
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<bool> UpdateStockAsync(int productId, int cantidad);
    }
}
