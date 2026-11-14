namespace SistemaPacientes.Infrastructure.Exceptions;

public class RecetaDetalleException : SistemaPacientesException
{
    public RecetaDetalleException(string message, Exception? inner = null) : base(message, inner) { }
}
