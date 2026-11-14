using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class SeguroRepository : BaseRepository<Seguro>, ISeguroRepository
{
    public SeguroRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Seguro> AddAsync(Seguro entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new SeguroException("No se pudo guardar el registro de seguros (ars).", ex); }
    }

    public override async Task UpdateAsync(Seguro entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new SeguroException("No se pudo actualizar el registro de seguros (ars).", ex); }
    }

    public async Task<bool> ExistsByNombreAsync(string valor, int? excludeId = null) =>
        await _set.AnyAsync(x => !x.Deleted && x.Nombre.ToLower() == valor.ToLower() && (excludeId == null || x.Id != excludeId));
}
