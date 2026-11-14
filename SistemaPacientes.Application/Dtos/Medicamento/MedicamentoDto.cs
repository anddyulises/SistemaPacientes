namespace SistemaPacientes.Application.Dtos;

public class MedicamentoDto : AddMedicamentoDto
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}
