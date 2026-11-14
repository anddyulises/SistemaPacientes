namespace SistemaPacientes.Infrastructure.Exceptions;

public class CitaException : SistemaPacientesException
{
    public CitaException(string message, Exception? inner = null) : base(message, inner) { }
}
