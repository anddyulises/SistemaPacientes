namespace SistemaPacientes.Application.Dtos;

public class AddRecetaDto
{
    public int ConsultaId { get; set; }
    public DateTime Fecha { get; set; }
    public string? Indicaciones { get; set; }
}
