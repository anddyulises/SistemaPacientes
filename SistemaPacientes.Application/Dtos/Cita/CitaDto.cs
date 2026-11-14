namespace SistemaPacientes.Application.Dtos;

public class CitaDto : AddCitaDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
