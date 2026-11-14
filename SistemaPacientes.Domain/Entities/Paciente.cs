using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Paciente : BaseEntity
{
    public string Cedula { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public int? SeguroId { get; set; }
}
