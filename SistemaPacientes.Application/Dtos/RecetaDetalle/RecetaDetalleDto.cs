namespace SistemaPacientes.Application.Dtos;

public class RecetaDetalleDto : AddRecetaDetalleDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
