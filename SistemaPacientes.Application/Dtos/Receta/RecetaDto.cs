namespace SistemaPacientes.Application.Dtos;

public class RecetaDto : AddRecetaDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
