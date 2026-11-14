using Microsoft.EntityFrameworkCore;
using SistemaPacientes.Domain.Core;
using SistemaPacientes.Domain.Repository;
using SistemaPacientes.Infrastructure.Context;

namespace SistemaPacientes.Infrastructure.Core;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly SistemaPacientesContext _context;
    protected readonly DbSet<T> _set;

    public BaseRepository(SistemaPacientesContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await _set.Where(e => !e.Deleted).ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) =>
        await _set.FirstOrDefaultAsync(e => e.Id == id && !e.Deleted);

    public virtual async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _set.FindAsync(id);
        if (entity is null) return;
        entity.Deleted = true; // borrado lógico
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(int id) =>
        await _set.AnyAsync(e => e.Id == id && !e.Deleted);
}
