namespace SistemaPacientes.Application.Dtos;

public class PacienteDto : AddPacienteDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
