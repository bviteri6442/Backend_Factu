using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// User entity for MongoDB with authentication
    /// </summary>
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("cedula")]
        [BsonRequired]
        public string Cedula { get; set; } = string.Empty;
        
        [BsonElement("correo")]
        [BsonRequired]
        public string Correo { get; set; } = string.Empty;
        
        [BsonElement("nombreCompleto")]
        [BsonRequired]
        public string NombreCompleto { get; set; } = string.Empty;
        
        [BsonElement("contrasenaHash")]
        [BsonRequired]
        public string ContrasenaHash { get; set; } = string.Empty; // BCrypt hash
        
        [BsonElement("activo")]
        public bool Activo { get; set; } = true;
        
        [BsonElement("fechaBloqueo")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? FechaBloqueo { get; set; }
        
        [BsonElement("razonBloqueo")]
        public string RazonBloqueo { get; set; } = string.Empty;
        
        [BsonElement("fechaCreacion")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        [BsonElement("fechaUltimoLogin")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? FechaUltimoLogin { get; set; }
        
        [BsonElement("rolId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RolId { get; set; } = string.Empty;
        
        [BsonElement("rolNombre")]
        public string RolNombre { get; set; } = string.Empty; // Desnormalizado para consultas rápidas
    }
}
