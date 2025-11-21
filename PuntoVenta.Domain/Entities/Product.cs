using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Product entity for MongoDB
    /// </summary>
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("nombre")]
        [BsonRequired]
        public string Nombre { get; set; } = string.Empty;
        
        [BsonElement("codigoBarra")]
        [BsonRequired]
        public string CodigoBarra { get; set; } = string.Empty;
        
        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        
        [BsonElement("precioCosto")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PrecioCosto { get; set; }
        
        [BsonElement("precioVenta")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PrecioVenta { get; set; }
        
        [BsonElement("stockActual")]
        public int StockActual { get; set; }
        
        [BsonElement("stockMinimo")]
        public int StockMinimo { get; set; } = 10;
        
        [BsonElement("activo")]
        public bool Activo { get; set; } = true;
        
        [BsonElement("fechaCreacion")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        [BsonElement("fechaActualizacion")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Índices únicos se configurarán en MongoDB
    }
}
