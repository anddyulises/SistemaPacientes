namespace SistemaPacientes.Application.Dtos;

public class AddConsultorioDto
{
    public string Codigo { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public int Piso { get; set; }
}
