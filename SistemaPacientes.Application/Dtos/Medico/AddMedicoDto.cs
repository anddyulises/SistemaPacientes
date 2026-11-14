namespace SistemaPacientes.Application.Dtos;

public class AddMedicoDto
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Exequatur { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public int EspecialidadId { get; set; }
}
