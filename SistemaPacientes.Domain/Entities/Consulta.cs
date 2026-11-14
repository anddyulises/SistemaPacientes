using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Consulta : BaseEntity
{
    public int CitaId { get; set; }
    public int DiagnosticoId { get; set; }
    public string? Notas { get; set; }
    public decimal Costo { get; set; }
}
