using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Medico : BaseEntity
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Exequatur { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public int EspecialidadId { get; set; }
}
