using MongoDB.Driver;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB repository for Cliente entity
    /// </summary>
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(MongoDbContext context) 
            : base(context, "clientes")
        {
        }

        public async Task<Cliente?> GetByDocumentoAsync(string documento)
        {
            var filter = Builders<Cliente>.Filter.And(
                Builders<Cliente>.Filter.Eq(c => c.Documento, documento),
                Builders<Cliente>.Filter.Eq(c => c.Activo, true)
            );
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var filterActive = Builders<Cliente>.Filter.Eq(c => c.Activo, true);
                return await _collection.Find(filterActive).ToListAsync();
            }

            var filter = Builders<Cliente>.Filter.And(
                Builders<Cliente>.Filter.Eq(c => c.Activo, true),
                Builders<Cliente>.Filter.Or(
                    Builders<Cliente>.Filter.Regex(c => c.Nombre, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Cliente>.Filter.Regex(c => c.Documento, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Cliente>.Filter.Regex(c => c.Email, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Cliente>.Filter.Regex(c => c.Telefono, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                )
            );

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
