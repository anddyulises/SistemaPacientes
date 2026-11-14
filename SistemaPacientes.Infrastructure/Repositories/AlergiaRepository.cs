using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class AlergiaRepository : BaseRepository<Alergia>, IAlergiaRepository
{
    public AlergiaRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Alergia> AddAsync(Alergia entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new AlergiaException("No se pudo guardar el registro de alergias.", ex); }
    }

    public override async Task UpdateAsync(Alergia entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new AlergiaException("No se pudo actualizar el registro de alergias.", ex); }
    }
}
