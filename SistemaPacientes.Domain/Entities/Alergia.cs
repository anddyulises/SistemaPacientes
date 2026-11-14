using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Alergia : BaseEntity
{
    public int PacienteId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
}
