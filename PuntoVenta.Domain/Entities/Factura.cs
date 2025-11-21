using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Invoice (Factura) entity for MongoDB
    /// Represents a complete sale transaction with embedded details
    /// </summary>
    public class Factura
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("numeroFactura")]
        [BsonRequired]
        public string NumeroFactura { get; set; } = string.Empty;
        
        [BsonElement("fechaVenta")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaVenta { get; set; } = DateTime.UtcNow;
        
        [BsonElement("usuarioId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = string.Empty;
        
        [BsonElement("usuarioNombre")]
        public string UsuarioNombre { get; set; } = string.Empty; // Denormalized for fast queries
        
        [BsonElement("clienteId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ClienteId { get; set; }
        
        [BsonElement("clienteNombre")]
        public string ClienteNombre { get; set; } = string.Empty;
        
        [BsonElement("clienteDocumento")]
        public string ClienteDocumento { get; set; } = string.Empty;
        
        [BsonElement("subtotal")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Subtotal { get; set; }
        
        [BsonElement("porcentajeIVA")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PorcentajeIVA { get; set; } = 19m;
        
        [BsonElement("totalImpuesto")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalImpuesto { get; set; }
        
        [BsonElement("totalVenta")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalVenta { get; set; }
        
        [BsonElement("estado")]
        public string Estado { get; set; } = "Completada"; // Completada, Pendiente, Cancelada
        
        [BsonElement("observaciones")]
        public string Observaciones { get; set; } = string.Empty;
        
        [BsonElement("detalles")]
        public List<DetalleFactura> Detalles { get; set; } = new();
    }

    /// <summary>
    /// Invoice detail (embedded document in MongoDB)
    /// Equivalent to DetalleVenta but embedded inside Factura
    /// </summary>
    public class DetalleFactura
    {
        [BsonElement("productoId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductoId { get; set; } = string.Empty;
        
        [BsonElement("productoNombre")]
        public string ProductoNombre { get; set; } = string.Empty;
        
        [BsonElement("codigoBarra")]
        public string CodigoBarra { get; set; } = string.Empty;
        
        [BsonElement("cantidad")]
        public int Cantidad { get; set; }
        
        [BsonElement("precioUnitario")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal PrecioUnitario { get; set; }
        
        [BsonElement("descuento")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Descuento { get; set; } = 0m;
        
        [BsonElement("total")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Total { get; set; }
    }
}