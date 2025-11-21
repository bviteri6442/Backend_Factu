using MongoDB.Driver;
using MongoDB.Bson;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Infrastructure.Persistencia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// Generic MongoDB repository implementation
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly MongoDbContext _context;

        public GenericRepository(MongoDbContext context, string collectionName)
        {
            _context = context;
            _collection = context.GetCollection<T>(collectionName);
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<string> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            
            // Extract the generated ObjectId
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity);
                return idValue?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity)?.ToString();
                if (!string.IsNullOrEmpty(idValue))
                {
                    var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(idValue));
                    await _collection.ReplaceOneAsync(filter, entity);
                }
            }
        }

        public virtual async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.DeleteOneAsync(filter);
        }

        public virtual async Task<bool> ExistsAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}