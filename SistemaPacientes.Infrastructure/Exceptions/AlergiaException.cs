namespace SistemaPacientes.Infrastructure.Exceptions;

public class AlergiaException : SistemaPacientesException
{
    public AlergiaException(string message, Exception? inner = null) : base(message, inner) { }
}
