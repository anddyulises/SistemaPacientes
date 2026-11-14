namespace SistemaPacientes.Infrastructure.Exceptions;

public class EspecialidadException : SistemaPacientesException
{
    public EspecialidadException(string message, Exception? inner = null) : base(message, inner) { }
}
