using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Consultorio : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public int Piso { get; set; }
}
