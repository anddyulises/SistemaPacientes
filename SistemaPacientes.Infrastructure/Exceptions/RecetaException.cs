namespace SistemaPacientes.Infrastructure.Exceptions;

public class RecetaException : SistemaPacientesException
{
    public RecetaException(string message, Exception? inner = null) : base(message, inner) { }
}
