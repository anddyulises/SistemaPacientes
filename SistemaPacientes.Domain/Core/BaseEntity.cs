namespace SistemaPacientes.Domain.Core;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public bool Deleted { get; set; }
}
