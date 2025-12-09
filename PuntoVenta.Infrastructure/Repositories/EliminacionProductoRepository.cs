using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;

namespace PuntoVenta.Infrastructure.Repositories
{
    public class EliminacionProductoRepository : GenericRepository<EliminacionProducto>, IEliminacionProductoRepository
    {
        public EliminacionProductoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EliminacionProducto>> BuscarAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _context.Set<EliminacionProducto>()
                    .OrderByDescending(e => e.FechaEliminacion)
                    .ToListAsync();
            }

            var term = searchTerm.ToLower();
            return await _context.Set<EliminacionProducto>()
                .Where(e => 
                    EF.Functions.ILike(e.NombreProductoEliminado, $"%{term}%") ||
                    EF.Functions.ILike(e.CodigoProductoEliminado, $"%{term}%") ||
                    EF.Functions.ILike(e.NombreAdministrador, $"%{term}%") ||
                    EF.Functions.ILike(e.MotivoEliminacion ?? "", $"%{term}%"))
                .OrderByDescending(e => e.FechaEliminacion)
                .ToListAsync();
        }
    }
}
