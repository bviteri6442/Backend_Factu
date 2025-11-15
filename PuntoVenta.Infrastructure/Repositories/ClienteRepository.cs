using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para gestionar Clientes
    /// </summary>
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Cliente> GetByDocumentoAsync(string documento)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Documento == documento && c.Activo);
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _dbSet.Where(c => c.Activo).ToListAsync();

            searchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(c => c.Activo &&
                    (c.Nombre.ToLower().Contains(searchTerm) ||
                     c.Documento.ToLower().Contains(searchTerm) ||
                     c.Email.ToLower().Contains(searchTerm) ||
                     c.Telefono.ToLower().Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
