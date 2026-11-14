using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Diagnostico : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoCie10 { get; set; }
    public string? Descripcion { get; set; }
}
