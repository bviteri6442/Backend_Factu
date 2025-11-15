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
    /// Repositorio para gestionar Usuarios
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<Usuario> _dbSet;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Usuario>();
        }

        public async Task<Usuario> GetByIdAsync(string id)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _dbSet
                .Include(u => u.Rol)
                .Where(u => u.Activo)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Usuario entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return 1; // Usuarios no tienen ID int
        }

        public async Task UpdateAsync(Usuario entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<Usuario> GetByCorreoAsync(string correo)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo);
        }

        public async Task<Usuario> GetByCedulaAsync(string cedula)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Cedula == cedula && u.Activo);
        }

        public async Task<IEnumerable<Usuario>> GetByRolAsync(int rolId)
        {
            return await _dbSet
                .Where(u => u.RolId == rolId && u.Activo)
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _dbSet.AnyAsync(u => u.Correo == correo);
        }

        public async Task<bool> ExisteCedulaAsync(string cedula)
        {
            return await _dbSet.AnyAsync(u => u.Cedula == cedula);
        }
    }
}
