namespace SistemaPacientes.Infrastructure.Exceptions;

// Excepción base: todas las excepciones del sistema heredan de ella.
public class SistemaPacientesException : Exception
{
    public SistemaPacientesException(string message, Exception? inner = null) : base(message, inner) { }
}
