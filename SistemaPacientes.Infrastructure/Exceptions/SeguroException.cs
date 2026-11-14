namespace SistemaPacientes.Infrastructure.Exceptions;

public class SeguroException : SistemaPacientesException
{
    public SeguroException(string message, Exception? inner = null) : base(message, inner) { }
}
