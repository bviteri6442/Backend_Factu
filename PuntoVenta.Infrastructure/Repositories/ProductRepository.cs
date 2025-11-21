using MongoDB.Driver;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB repository for Product entity
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(MongoDbContext context) 
            : base(context, "products")
        {
        }

        public async Task<Product?> GetByCodigoBarraAsync(string codigoBarra)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.CodigoBarra, codigoBarra);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductosConStockAsync()
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(p => p.Activo, true),
                Builders<Product>.Filter.Gt(p => p.StockActual, 0)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetProductosConStockAsync();
            }

            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(p => p.Activo, true),
                Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(p => p.Nombre, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Product>.Filter.Regex(p => p.CodigoBarra, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Product>.Filter.Regex(p => p.Descripcion, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                )
            );

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateStockAsync(string productId, int cantidad)
        {
            var filter = Builders<Product>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(productId));
            var update = Builders<Product>.Update
                .Inc(p => p.StockActual, cantidad)
                .Set(p => p.FechaActualizacion, System.DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}

