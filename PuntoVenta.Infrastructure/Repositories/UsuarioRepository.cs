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
    /// EF Core repository for Usuario entity
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Activo)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Usuario entity)
        {
            await _context.Usuarios.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(Usuario entity)
        {
            _context.Usuarios.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == correo && u.Activo);
        }

        public async Task<Usuario?> GetByCedulaAsync(string cedula)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == cedula && u.Activo);
        }

        public async Task<IEnumerable<Usuario>> GetByRolAsync(int rolId)
        {
            return await _context.Usuarios
                .Where(u => u.RolId == rolId && u.Activo)
                .ToListAsync();
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == correo);
        }

        public async Task<bool> ExisteCedulaAsync(string cedula)
        {
            return await _context.Usuarios.AnyAsync(u => u.NombreUsuario == cedula);
        }
    }
}
