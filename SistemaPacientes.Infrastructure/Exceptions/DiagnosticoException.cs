namespace SistemaPacientes.Infrastructure.Exceptions;

public class DiagnosticoException : SistemaPacientesException
{
    public DiagnosticoException(string message, Exception? inner = null) : base(message, inner) { }
}
