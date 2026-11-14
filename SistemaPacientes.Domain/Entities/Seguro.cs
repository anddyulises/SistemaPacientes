using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Seguro : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public decimal Cobertura { get; set; }
}
