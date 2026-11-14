namespace SistemaPacientes.Infrastructure.Exceptions;

public class ConsultorioException : SistemaPacientesException
{
    public ConsultorioException(string message, Exception? inner = null) : base(message, inner) { }
}
