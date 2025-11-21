using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad para registrar todos los errores ocurridos en el sistema
    /// </summary>
    public class ErrorLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        /// <summary>
        /// Tipo de excepción
        /// </summary>
        [BsonElement("tipoError")]
        public string TipoError { get; set; } = string.Empty;
        
        /// <summary>
        /// Mensaje de error
        /// </summary>
        [BsonElement("mensaje")]
        [BsonRequired]
        public string Mensaje { get; set; } = string.Empty;
        
        /// <summary>
        /// Stack trace del error
        /// </summary>
        [BsonElement("stackTrace")]
        public string StackTrace { get; set; } = string.Empty;
        
        /// <summary>
        /// Fuente del error (clase, método)
        /// </summary>
        [BsonElement("fuente")]
        public string Fuente { get; set; } = string.Empty; // Controller/Service name
        
        /// <summary>
        /// Número de línea donde ocurrió el error
        /// </summary>
        [BsonElement("numeroLinea")]
        public int? NumeroLinea { get; set; }
        
        /// <summary>
        /// Pantalla o endpoint donde ocurrió el error
        /// </summary>
        [BsonElement("pantalla")]
        public string Pantalla { get; set; } = string.Empty; // Endpoint path
        
        /// <summary>
        /// Usuario que experimentó el error
        /// </summary>
        [BsonElement("usuarioId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UsuarioId { get; set; }
        
        /// <summary>
        /// Evento que generó el error (ej: "CrearVenta", "LoginUsuario")
        /// </summary>
        [BsonElement("evento")]
        public string Evento { get; set; } = string.Empty; // "CreateInvoice", "Login", etc.
        
        /// <summary>
        /// Datos adicionales contextuales
        /// </summary>
        [BsonElement("datosAdicionales")]
        public string DatosAdicionales { get; set; } = string.Empty;
        
        /// <summary>
        /// Nivel de severidad (Info, Warning, Error, Critical)
        /// </summary>
        [BsonElement("nivelSeveridad")]
        public string NivelSeveridad { get; set; } = "Error"; // Error, Warning, Critical
        
        /// <summary>
        /// Fecha y hora del error
        /// </summary>
        [BsonElement("fechaError")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaError { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Indicador si el error fue revisado
        /// </summary>
        [BsonElement("revisado")]
        public bool Revisado { get; set; } = false;
        
        /// <summary>
        /// Notas de soporte técnico
        /// </summary>
        [BsonElement("notasSoporte")]
        public string NotasSoporte { get; set; } = string.Empty;
    }
}
