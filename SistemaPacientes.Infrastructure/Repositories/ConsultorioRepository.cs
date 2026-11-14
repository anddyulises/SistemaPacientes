using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class ConsultorioRepository : BaseRepository<Consultorio>, IConsultorioRepository
{
    public ConsultorioRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Consultorio> AddAsync(Consultorio entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new ConsultorioException("No se pudo guardar el registro de consultorios.", ex); }
    }

    public override async Task UpdateAsync(Consultorio entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new ConsultorioException("No se pudo actualizar el registro de consultorios.", ex); }
    }

    public async Task<bool> ExistsByCodigoAsync(string valor, int? excludeId = null) =>
        await _set.AnyAsync(x => !x.Deleted && x.Codigo.ToLower() == valor.ToLower() && (excludeId == null || x.Id != excludeId));
}
