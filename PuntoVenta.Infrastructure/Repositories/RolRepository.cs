using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository for Rol entity
    /// </summary>
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Rol?> GetByNombreAsync(string nombre)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre == nombre && r.Activo);
        }
    }
}
