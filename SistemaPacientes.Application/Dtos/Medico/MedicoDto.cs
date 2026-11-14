namespace SistemaPacientes.Application.Dtos;

public class MedicoDto : AddMedicoDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
