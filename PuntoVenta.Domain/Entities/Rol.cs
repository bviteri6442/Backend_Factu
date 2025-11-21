using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Role entity for RBAC (Role-Based Access Control)
    /// </summary>
    public class Rol
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("nombre")]
        [BsonRequired]
        public string Nombre { get; set; } = string.Empty; // "Administrador", "Usuario"
        
        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        
        [BsonElement("activo")]
        public bool Activo { get; set; } = true;
        
        [BsonElement("fechaCreacion")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        [BsonElement("permisos")]
        public List<string> Permisos { get; set; } = new(); // ["crear_factura", "ver_reportes", etc.]
    }
}
