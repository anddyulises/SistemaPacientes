using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Domain.Repository;

namespace SistemaPacientes.Infrastructure.Interfaces;

public interface IConsultorioRepository : IBaseRepository<Consultorio>
{
    Task<bool> ExistsByCodigoAsync(string valor, int? excludeId = null);
}
