namespace PuntoVenta.Domain.Entities
{
    /// <summary>
    /// Login attempts tracking for account lockout (3 failed attempts)
    /// </summary>
    public class IntentosLogin
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public bool Exitoso { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime FechaIntento { get; set; } = DateTime.UtcNow;
        public string? MensajeError { get; set; }
    }
}
