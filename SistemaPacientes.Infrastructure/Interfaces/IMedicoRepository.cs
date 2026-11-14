using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Domain.Repository;

namespace SistemaPacientes.Infrastructure.Interfaces;

public interface IMedicoRepository : IBaseRepository<Medico>
{
    Task<bool> ExistsByExequaturAsync(string valor, int? excludeId = null);
}
