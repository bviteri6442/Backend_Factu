using MongoDB.Driver;
using MongoDB.Bson;
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
    /// MongoDB repository for Factura (Invoice) entity
    /// </summary>
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(MongoDbContext context) 
            : base(context, "facturas")
        {
        }

        public async Task<Factura?> GetFacturaConDetallesAsync(string id)
        {
            // MongoDB automatically includes embedded documents (Detalles)
            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorFechaAsync(DateTime desde, DateTime hasta)
        {
            var filter = Builders<Factura>.Filter.And(
                Builders<Factura>.Filter.Gte(f => f.FechaVenta, desde),
                Builders<Factura>.Filter.Lte(f => f.FechaVenta, hasta)
            );

            return await _collection.Find(filter)
                .SortByDescending(f => f.FechaVenta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorUsuarioAsync(string usuarioId)
        {
            var filter = Builders<Factura>.Filter.Eq(f => f.UsuarioId, usuarioId);
            return await _collection.Find(filter)
                .SortByDescending(f => f.FechaVenta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorClienteAsync(string clienteId)
        {
            var filter = Builders<Factura>.Filter.Eq(f => f.ClienteId, clienteId);
            return await _collection.Find(filter)
                .SortByDescending(f => f.FechaVenta)
                .ToListAsync();
        }

        public async Task<string> GenerarNumeroFacturaAsync()
        {
            // Get the latest invoice number
            var lastFactura = await _collection.Find(_ => true)
                .SortByDescending(f => f.NumeroFactura)
                .Limit(1)
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
            var filter = Builders<Factura>.Filter.Eq(f => f.NumeroFactura, numeroFactura);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}