namespace SistemaPacientes.Application.Dtos;

public class AddConsultaDto
{
    public int CitaId { get; set; }
    public int DiagnosticoId { get; set; }
    public string? Notas { get; set; }
    public decimal Costo { get; set; }
}
