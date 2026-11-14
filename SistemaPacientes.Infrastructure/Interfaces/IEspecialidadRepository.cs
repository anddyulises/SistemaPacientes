using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Domain.Repository;

namespace SistemaPacientes.Infrastructure.Interfaces;

public interface IEspecialidadRepository : IBaseRepository<Especialidad>
{
    Task<bool> ExistsByNombreAsync(string valor, int? excludeId = null);
}
