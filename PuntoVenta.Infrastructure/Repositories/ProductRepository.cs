using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository for Product entity
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Product?> GetByCodigoBarraAsync(string codigoBarra)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(p => p.Codigo == codigoBarra);
        }

        public async Task<IEnumerable<Product>> GetProductosConStockAsync()
        {
            return await _context.Productos
                .Where(p => p.Activo && p.Stock > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetProductosConStockAsync();
            }

            return await _context.Productos
                .Where(p => p.Activo && (
                    EF.Functions.ILike(p.Nombre, $"%{searchTerm}%") ||
                    EF.Functions.ILike(p.Codigo, $"%{searchTerm}%") ||
                    EF.Functions.ILike(p.Descripcion ?? "", $"%{searchTerm}%")
                ))
                .ToListAsync();
        }

        public async Task<bool> UpdateStockAsync(int productId, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(productId);
            if (producto == null)
            {
                return false;
            }

            producto.Stock += cantidad;
            producto.FechaActualizacion = System.DateTime.UtcNow;
            
            _context.Productos.Update(producto);
            return true;
        }
    }
}
