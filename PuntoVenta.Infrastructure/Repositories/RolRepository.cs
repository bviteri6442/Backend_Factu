using MongoDB.Driver;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB repository for Rol entity
    /// </summary>
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(MongoDbContext context) 
            : base(context, "roles")
        {
        }

        public async Task<Rol?> GetByNombreAsync(string nombre)
        {
            var filter = Builders<Rol>.Filter.And(
                Builders<Rol>.Filter.Eq(r => r.Nombre, nombre),
                Builders<Rol>.Filter.Eq(r => r.Activo, true)
            );
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
