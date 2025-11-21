using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Login attempts tracking for account lockout (3 failed attempts)
    /// </summary>
    public class IntentosLogin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("correo")]
        [BsonRequired]
        public string Correo { get; set; } = string.Empty;
        
        [BsonElement("numeroIntentosFallidos")]
        public int NumeroIntentosFallidos { get; set; } = 0;
        
        [BsonElement("fechaUltimoIntento")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaUltimoIntento { get; set; } = DateTime.UtcNow;
        
        [BsonElement("direccionIP")]
        public string DireccionIP { get; set; } = string.Empty;
        
        [BsonElement("userAgent")]
        public string UserAgent { get; set; } = string.Empty;
        
        [BsonElement("bloqueado")]
        public bool Bloqueado { get; set; } = false;
        
        [BsonElement("fechaBloqueo")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? FechaBloqueo { get; set; }
    }
}
