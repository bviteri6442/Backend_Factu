using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository for Factura (Invoice) entity
    /// </summary>
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Factura?> GetFacturaConDetallesAsync(int id)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorFechaAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .Where(f => f.FechaVenta >= desde && f.FechaVenta <= hasta)
                .OrderByDescending(f => f.FechaVenta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorUsuarioAsync(int usuarioId)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .Where(f => f.UsuarioId == usuarioId)
                .OrderByDescending(f => f.FechaVenta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorClienteAsync(int clienteId)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .Where(f => f.ClienteId == clienteId)
                .OrderByDescending(f => f.FechaVenta)
                .ToListAsync();
        }

        public async Task<string> GenerarNumeroFacturaAsync()
        {
            var lastFactura = await _context.Facturas
                .OrderByDescending(f => f.NumeroFactura)
                .FirstOrDefaultAsync();

            if (lastFactura == null)
            {
                return "FAC-00001";
            }

            // Extract the numeric part and increment
            var lastNumber = lastFactura.NumeroFactura.Replace("FAC-", "");
            if (int.TryParse(lastNumber, out int number))
            {
                return $"FAC-{(number + 1):D5}";
            }

            return "FAC-00001";
        }

        public async Task<Factura?> GetByNumeroFacturaAsync(string numeroFactura)
        {
            return await _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);
        }
    }
}
