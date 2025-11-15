using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene un producto por su código de barras
        /// </summary>
        public async Task<Product> GetByCodigoBarraAsync(string codigoBarra)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.CodigoBarra == codigoBarra && p.Activo);
        }

        /// <summary>
        /// Obtiene todos los productos que tienen stock disponible
        /// </summary>
        public async Task<IEnumerable<Product>> GetProductosConStockAsync()
        {
            return await _dbSet.Where(p => p.Activo && p.StockActual > 0).ToListAsync();
        }

        /// <summary>
        /// Búsqueda inteligente de productos por nombre, descripción o código de barras
        /// </summary>
        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetProductosConStockAsync();

            searchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(p => p.Activo && p.StockActual > 0 &&
                    (p.Nombre.ToLower().Contains(searchTerm) ||
                     p.Descripcion.ToLower().Contains(searchTerm) ||
                     p.CodigoBarra.ToLower().Contains(searchTerm)))
                .ToListAsync();
        }

        /// <summary>
        /// Actualiza el stock de un producto
        /// </summary>
        public async Task UpdateStockAsync(int productId, int cantidad)
        {
            var producto = await GetByIdAsync(productId);
            if (producto != null)
            {
                producto.StockActual -= cantidad;
                producto.FechaActualizacion = DateTime.UtcNow;
                await UpdateAsync(producto);
            }
        }

        /// <summary>
        /// Obtiene todos los productos (incluyendo inactivos y sin stock) - para administración
        /// </summary>
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}

