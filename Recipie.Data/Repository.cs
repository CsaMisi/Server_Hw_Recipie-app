using Microsoft.EntityFrameworkCore;
using Recipie.Model.Entity;

namespace Recipie.Data;

public class Repository<T> where T : class, IIdEntity
{
    private readonly RecipeAppContext _context;

    public Repository(RecipeAppContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(string id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            await DeleteAsync(entity);
        }
    }
}
