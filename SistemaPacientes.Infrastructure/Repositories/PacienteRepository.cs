using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
{
    public PacienteRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Paciente> AddAsync(Paciente entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new PacienteException("No se pudo guardar el registro de pacientes.", ex); }
    }

    public override async Task UpdateAsync(Paciente entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new PacienteException("No se pudo actualizar el registro de pacientes.", ex); }
    }

    public async Task<bool> ExistsByCedulaAsync(string valor, int? excludeId = null) =>
        await _set.AnyAsync(x => !x.Deleted && x.Cedula.ToLower() == valor.ToLower() && (excludeId == null || x.Id != excludeId));
}
