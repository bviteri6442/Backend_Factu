using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para gestionar Ventas
    /// </summary>
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        public VentaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Venta> GetVentaConDetallesAsync(int id)
        {
            return await _dbSet
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venta>> GetVentasPorFechaAsync(DateTime desde, DateTime hasta)
        {
            return await _dbSet
                .Where(v => v.FechaVenta >= desde && v.FechaVenta <= hasta)
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venta>> GetVentasPorUsuarioAsync(string usuarioId)
        {
            return await _dbSet
                .Where(v => v.UsuarioId == usuarioId)
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venta>> GetVentasPorClienteAsync(int clienteId)
        {
            return await _dbSet
                .Where(v => v.ClienteId == clienteId)
                .Include(v => v.Detalles)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();
        }

        public async Task<string> GenerarNumeroFacturaAsync()
        {
            var ultimaVenta = await _dbSet
                .OrderByDescending(v => v.Id)
                .FirstOrDefaultAsync();

            int numero = (ultimaVenta?.Id ?? 0) + 1;
            return $"FAC-{DateTime.Now:yyyyMMdd}-{numero.ToString("D6")}";
        }
    }
}
