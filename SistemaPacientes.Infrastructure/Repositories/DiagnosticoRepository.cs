using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class DiagnosticoRepository : BaseRepository<Diagnostico>, IDiagnosticoRepository
{
    public DiagnosticoRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Diagnostico> AddAsync(Diagnostico entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new DiagnosticoException("No se pudo guardar el registro de diagnósticos.", ex); }
    }

    public override async Task UpdateAsync(Diagnostico entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new DiagnosticoException("No se pudo actualizar el registro de diagnósticos.", ex); }
    }
}
