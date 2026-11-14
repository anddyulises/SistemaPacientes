using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Entities;
using SistemaPacientes.Infrastructure.Context;
using SistemaPacientes.Infrastructure.Core;
using SistemaPacientes.Infrastructure.Exceptions;
using SistemaPacientes.Infrastructure.Interfaces;

namespace SistemaPacientes.Infrastructure.Repositories;

public class ConsultaRepository : BaseRepository<Consulta>, IConsultaRepository
{
    public ConsultaRepository(SistemaPacientesContext context) : base(context) { }

    public override async Task<Consulta> AddAsync(Consulta entity)
    {
        try { return await base.AddAsync(entity); }
        catch (DbUpdateException ex) { throw new ConsultaException("No se pudo guardar el registro de consultas.", ex); }
    }

    public override async Task UpdateAsync(Consulta entity)
    {
        try { await base.UpdateAsync(entity); }
        catch (DbUpdateException ex) { throw new ConsultaException("No se pudo actualizar el registro de consultas.", ex); }
    }
}
