using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Receta : BaseEntity
{
    public int ConsultaId { get; set; }
    public DateTime Fecha { get; set; }
    public string? Indicaciones { get; set; }
}
