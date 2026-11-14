namespace SistemaPacientes.Infrastructure.Exceptions;

public class MedicamentoException : SistemaPacientesException
{
    public MedicamentoException(string message, Exception? inner = null) : base(message, inner) { }
}
