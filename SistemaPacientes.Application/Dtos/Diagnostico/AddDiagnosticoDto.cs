namespace SistemaPacientes.Application.Dtos;

public class AddDiagnosticoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoCie10 { get; set; }
    public string? Descripcion { get; set; }
}
