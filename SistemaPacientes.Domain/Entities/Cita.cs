using SistemaPacientes.Domain.Core;

namespace SistemaPacientes.Domain.Entities;

public class Cita : BaseEntity
{
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public int ConsultorioId { get; set; }
    public DateTime FechaHora { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
