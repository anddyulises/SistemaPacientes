using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
{
    public MedicoRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Medico> AddAsync(Medico entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new MedicoException("No se pudo guardar el registro de médicos.", ex); }
    }

    public override async Task UpdateAsync(Medico entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new MedicoException("No se pudo actualizar el registro de médicos.", ex); }
    }

    public async Task<bool> ExistsByExequaturAsync(string valor, int? excludeId = null) =>
        await _set.AnyAsync(x => !x.Deleted && x.Exequatur.ToLower() == valor.ToLower() && (excludeId == null || x.Id != excludeId));
}
