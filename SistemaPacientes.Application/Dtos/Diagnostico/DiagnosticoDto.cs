namespace SistemaPacientes.Application.Dtos;

public class DiagnosticoDto : AddDiagnosticoDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
