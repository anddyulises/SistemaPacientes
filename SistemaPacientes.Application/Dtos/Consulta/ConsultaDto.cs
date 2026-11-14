namespace SistemaPacientes.Application.Dtos;

public class ConsultaDto : AddConsultaDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
