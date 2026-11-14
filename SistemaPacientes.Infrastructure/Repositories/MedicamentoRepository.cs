using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class MedicamentoRepository : BaseRepository<Medicamento>, IMedicamentoRepository
{
    public MedicamentoRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Medicamento> AddAsync(Medicamento entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new MedicamentoException("No se pudo guardar el registro de medicamentos.", ex); }
    }

    public override async Task UpdateAsync(Medicamento entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new MedicamentoException("No se pudo actualizar el registro de medicamentos.", ex); }
    }
}
