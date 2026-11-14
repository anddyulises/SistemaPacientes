namespace SistemaPacientes.Application.Dtos;

public class AddMedicamentoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Presentacion { get; set; }
    public int Stock { get; set; }
}
