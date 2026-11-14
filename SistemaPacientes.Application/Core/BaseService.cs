using SistemaPacientes.Infrastructure.Exceptions;

namespace SistemaPacientes.Application.Core;

public abstract class BaseService
{
    protected static ServiceResult Ok(object? data = null, string message = "Operación realizada correctamente.")
        => new() { Success = true, Message = message, Data = data };

    protected static ServiceResult Fail(string message)
        => new() { Success = false, Message = message };

    protected static ServiceResult Fail(IEnumerable<string> errors)
        => Fail(string.Join(" ", errors));

    protected static ServiceResult NotFound(string entidad)
        => new() { Success = false, NotFound = true, Message = $"{entidad} no encontrado." };

    // Ejecuta una operación y convierte las excepciones de negocio en un ServiceResult.
    protected static async Task<ServiceResult> ExecuteAsync(Func<Task<ServiceResult>> action)
    {
        try
        {
            return await action();
        }
        catch (SistemaPacientesException ex)
        {
            return Fail(ex.Message);
        }
    }
}
