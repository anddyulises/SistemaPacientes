using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class RecetaDetalleRepository : BaseRepository<RecetaDetalle>, IRecetaDetalleRepository
{
    public RecetaDetalleRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<RecetaDetalle> AddAsync(RecetaDetalle entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new RecetaDetalleException("No se pudo guardar el registro de detalle de receta.", ex); }
    }

    public override async Task UpdateAsync(RecetaDetalle entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new RecetaDetalleException("No se pudo actualizar el registro de detalle de receta.", ex); }
    }
}
