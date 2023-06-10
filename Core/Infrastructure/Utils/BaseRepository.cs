using System.Linq.Expressions;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utils;

class BaseRepository<T> : IBaseRepository<T>
  where T : BaseEntity
{
    protected readonly PishgamanContext Context;
    public BaseRepository(PishgamanContext context)
    {
        Context = context;
    }
    public async Task<T?> GetAsync(int id)
    {
        return await Context.Set<T>().FirstOrDefaultAsync(t => t.Id.Equals(id)); ;
    }
    public async Task<T?> GetTracking(int id)
    {
        return await Context.Set<T>().AsTracking().FirstOrDefaultAsync(t => t.Id.Equals(id));
    }
    public void Add(T entity)
    {
        Context.Set<T>().Add(entity);
    }
    public async Task AddRange(ICollection<T> entities)
    {
        await Context.Set<T>().AddRangeAsync(entities);
    }
    public void Update(T entity)
    {
        Context.Update(entity);
    }
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
    {
        return await Context.Set<T>().AnyAsync(expression);
    }
    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return Context.Set<T>().Any(expression);
    }
    public T? Get(int id)
    {
        return Context.Set<T>().FirstOrDefault(t => t.Id.Equals(id)); ;
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>> expression)
    {
        return Context.Set<T>().Where(expression);

    }
}