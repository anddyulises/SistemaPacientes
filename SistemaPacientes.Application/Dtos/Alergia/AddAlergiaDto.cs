namespace SistemaPacientes.Application.Dtos;

public class AddAlergiaDto
{
    public int PacienteId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
}
