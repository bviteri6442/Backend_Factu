using MongoDB.Driver;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using System;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB repository for IntentosLogin entity
    /// </summary>
    public class IntentosLoginRepository : GenericRepository<IntentosLogin>, IIntentosLoginRepository
    {
        public IntentosLoginRepository(MongoDbContext context) 
            : base(context, "intentosLogin")
        {
        }

        public async Task<IntentosLogin?> GetByCorreoAsync(string correo)
        {
            var filter = Builders<IntentosLogin>.Filter.Eq(il => il.Correo, correo);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task IncrementarIntentosAsync(string correo, string ip, string userAgent)
        {
            var intento = await GetByCorreoAsync(correo);

            if (intento == null)
            {
                intento = new IntentosLogin
                {
                    Correo = correo,
                    NumeroIntentosFallidos = 1,
                    FechaUltimoIntento = DateTime.UtcNow,
                    DireccionIP = ip,
                    UserAgent = userAgent
                };
                await _collection.InsertOneAsync(intento);
            }
            else
            {
                intento.NumeroIntentosFallidos++;
                intento.FechaUltimoIntento = DateTime.UtcNow;
                intento.DireccionIP = ip;
                intento.UserAgent = userAgent;

                // Block after 3 failed attempts
                if (intento.NumeroIntentosFallidos >= 3)
                {
                    intento.Bloqueado = true;
                    intento.FechaBloqueo = DateTime.UtcNow;
                }

                var filter = Builders<IntentosLogin>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(intento.Id));
                await _collection.ReplaceOneAsync(filter, intento);
            }
        }

        public async Task ReiniciarIntentosAsync(string correo)
        {
            var intento = await GetByCorreoAsync(correo);
            if (intento != null)
            {
                intento.NumeroIntentosFallidos = 0;
                intento.Bloqueado = false;
                intento.FechaBloqueo = null;
                
                var filter = Builders<IntentosLogin>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(intento.Id));
                await _collection.ReplaceOneAsync(filter, intento);
            }
        }

        public async Task BloquearUsuarioAsync(string correo)
        {
            var intento = await GetByCorreoAsync(correo);
            if (intento == null)
            {
                intento = new IntentosLogin
                {
                    Correo = correo,
                    NumeroIntentosFallidos = 3,
                    Bloqueado = true,
                    FechaBloqueo = DateTime.UtcNow,
                    FechaUltimoIntento = DateTime.UtcNow
                };
                await _collection.InsertOneAsync(intento);
            }
            else
            {
                intento.Bloqueado = true;
                intento.FechaBloqueo = DateTime.UtcNow;
                
                var filter = Builders<IntentosLogin>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(intento.Id));
                await _collection.ReplaceOneAsync(filter, intento);
            }
        }
    }
}
