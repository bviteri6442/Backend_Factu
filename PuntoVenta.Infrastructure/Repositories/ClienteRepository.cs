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
    /// EF Core repository for Cliente entity
    /// </summary>
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Cliente?> GetByDocumentoAsync(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Documento == documento && c.Activo);
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _context.Clientes
                    .Where(c => c.Activo)
                    .ToListAsync();
            }

            return await _context.Clientes
                .Where(c => c.Activo && (
                    EF.Functions.ILike(c.Nombre, $"%{searchTerm}%") ||
                    EF.Functions.ILike(c.Documento, $"%{searchTerm}%") ||
                    EF.Functions.ILike(c.Email ?? "", $"%{searchTerm}%") ||
                    EF.Functions.ILike(c.Telefono ?? "", $"%{searchTerm}%")
                ))
                .ToListAsync();
        }
    }
}
