using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class CitaRepository : BaseRepository<Cita>, ICitaRepository
{
    public CitaRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Cita> AddAsync(Cita entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new CitaException("No se pudo guardar el registro de citas.", ex); }
    }

    public override async Task UpdateAsync(Cita entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new CitaException("No se pudo actualizar el registro de citas.", ex); }
    }
}
