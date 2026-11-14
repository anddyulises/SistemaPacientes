namespace SistemaPacientes.Infrastructure.Exceptions;

public class PacienteException : SistemaPacientesException
{
    public PacienteException(string message, Exception? inner = null) : base(message, inner) { }
}
