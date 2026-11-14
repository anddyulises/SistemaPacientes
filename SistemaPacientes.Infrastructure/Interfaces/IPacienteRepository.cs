using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Domain.Repository;

namespace SistemaPacientes.Infrastructure.Interfaces;

public interface IPacienteRepository : IBaseRepository<Paciente>
{
    Task<bool> ExistsByCedulaAsync(string valor, int? excludeId = null);
}
