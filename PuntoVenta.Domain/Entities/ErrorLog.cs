namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Entidad para registrar todos los errores ocurridos en el sistema
    /// </summary>
    public class ErrorLog
    {
        public int Id { get; set; }
        public string TipoError { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? Origen { get; set; }
        public int? NumeroLinea { get; set; }
        public string? UsuarioId { get; set; }
        public string? Nivel { get; set; } = "Error";
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public bool Revisado { get; set; } = false;
        public string? Notas { get; set; }
    }
}
