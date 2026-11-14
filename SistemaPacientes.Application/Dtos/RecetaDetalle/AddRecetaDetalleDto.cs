namespace SistemaPacientes.Application.Dtos;

public class AddRecetaDetalleDto
{
    public int RecetaId { get; set; }
    public int MedicamentoId { get; set; }
    public string Dosis { get; set; } = string.Empty;
    public int DuracionDias { get; set; }
}
