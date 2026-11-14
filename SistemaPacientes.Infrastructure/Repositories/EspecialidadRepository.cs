using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class EspecialidadRepository : BaseRepository<Especialidad>, IEspecialidadRepository
{
    public EspecialidadRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Especialidad> AddAsync(Especialidad entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new EspecialidadException("No se pudo guardar el registro de especialidades.", ex); }
    }

    public override async Task UpdateAsync(Especialidad entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new EspecialidadException("No se pudo actualizar el registro de especialidades.", ex); }
    }

    public async Task<bool> ExistsByNombreAsync(string valor, int? excludeId = null) =>
        await _set.AnyAsync(x => !x.Deleted && x.Nombre.ToLower() == valor.ToLower() && (excludeId == null || x.Id != excludeId));
}
