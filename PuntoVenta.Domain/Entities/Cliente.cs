using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Client entity for MongoDB
    /// </summary>
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("nombre")]
        [BsonRequired]
        public string Nombre { get; set; } = string.Empty;
        
        [BsonElement("documento")]
        [BsonRequired]
        public string Documento { get; set; } = string.Empty;
        
        [BsonElement("direccion")]
        public string Direccion { get; set; } = string.Empty;
        
        [BsonElement("telefono")]
        public string Telefono { get; set; } = string.Empty;
        
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
        
        [BsonElement("activo")]
        public bool Activo { get; set; } = true;
        
        [BsonElement("fechaCreacion")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
