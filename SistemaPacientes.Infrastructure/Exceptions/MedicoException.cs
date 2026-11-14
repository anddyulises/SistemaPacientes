namespace SistemaPacientes.Infrastructure.Exceptions;

public class MedicoException : SistemaPacientesException
{
    public MedicoException(string message, Exception? inner = null) : base(message, inner) { }
}
