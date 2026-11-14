namespace SistemaPacientes.Infrastructure.Exceptions;

public class ConsultaException : SistemaPacientesException
{
    public ConsultaException(string message, Exception? inner = null) : base(message, inner) { }
}
