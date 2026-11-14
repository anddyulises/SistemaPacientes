using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Domain.Repository;

namespace SistemaPacientes.Infrastructure.Interfaces;

public interface ISeguroRepository : IBaseRepository<Seguro>
{
    Task<bool> ExistsByNombreAsync(string valor, int? excludeId = null);
}
