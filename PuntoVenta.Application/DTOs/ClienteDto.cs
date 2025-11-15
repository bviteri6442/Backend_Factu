using System.ComponentModel.DataAnnotations;

namespace PuntoVenta.Application.DTOs
{
    /// <summary>
    /// DTO para crear un Cliente
    /// </summary>
    public class CreateClienteDto
    {
        [Required(ErrorMessage = "El nombre del cliente es requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string? Nombre { get; set; }

        [Required(ErrorMessage = "El documento es requerido")]
        [RegularExpression(@"^\d{6,20}$", ErrorMessage = "El documento debe contener entre 6 y 20 dígitos")]
    public string? Documento { get; set; }

        [StringLength(300, ErrorMessage = "La dirección no puede exceder 300 caracteres")]
    public string? Direccion { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
    public string? Email { get; set; }
    }

    /// <summary>
    /// DTO para actualizar un Cliente
    /// </summary>
    public class UpdateClienteDto
    {
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string? Nombre { get; set; }

        [StringLength(300, ErrorMessage = "La dirección no puede exceder 300 caracteres")]
    public string? Direccion { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
    public string? Email { get; set; }

        public bool? Activo { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de Cliente
    /// </summary>
    public class ClienteResponseDto
    {
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Documento { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    }
}
