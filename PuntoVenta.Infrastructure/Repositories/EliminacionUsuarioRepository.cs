using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;

namespace PuntoVenta.Infrastructure.Repositories
{
    public class EliminacionUsuarioRepository : GenericRepository<EliminacionUsuario>, IEliminacionUsuarioRepository
    {
        public EliminacionUsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EliminacionUsuario>> BuscarAsync(string termino)
        {
            var terminoLower = termino.ToLower();
            return await _dbSet
                .Where(e =>
                    e.CedulaUsuarioEliminado.ToLower().Contains(terminoLower) ||
                    e.NombreUsuarioEliminado.ToLower().Contains(terminoLower) ||
                    e.EmailUsuarioEliminado.ToLower().Contains(terminoLower) ||
                    e.RolUsuarioEliminado.ToLower().Contains(terminoLower) ||
                    e.NombreAdministrador.ToLower().Contains(terminoLower) ||
                    (e.MotivoEliminacion != null && e.MotivoEliminacion.ToLower().Contains(terminoLower)) ||
                    e.TipoEliminacion.ToLower().Contains(terminoLower))
                .OrderByDescending(e => e.FechaEliminacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<EliminacionUsuario>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta)
        {
            return await _dbSet
                .Where(e => e.FechaEliminacion >= desde && e.FechaEliminacion <= hasta)
                .OrderByDescending(e => e.FechaEliminacion)
                .ToListAsync();
        }

        public override async Task<IEnumerable<EliminacionUsuario>> GetAllAsync()
        {
            return await _dbSet
                .OrderByDescending(e => e.FechaEliminacion)
                .ToListAsync();
        }
    }
}
