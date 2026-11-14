namespace SistemaPacientes.Application.Dtos;

public class AddSeguroDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public decimal Cobertura { get; set; }
}
