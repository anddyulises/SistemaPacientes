using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class RecetaRepository : BaseRepository<Receta>, IRecetaRepository
{
    public RecetaRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Receta> AddAsync(Receta entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new RecetaException("No se pudo guardar el registro de recetas.", ex); }
    }

    public override async Task UpdateAsync(Receta entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new RecetaException("No se pudo actualizar el registro de recetas.", ex); }
    }
}
