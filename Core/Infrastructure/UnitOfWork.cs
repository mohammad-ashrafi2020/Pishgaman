using Infrastructure.DataBase;
using Infrastructure.Entities.Persons;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// پیاده سازی از روی یه مثال توی Medium
/// </summary>
class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly PishgamanContext _dbContext;
    public UnitOfWork(PishgamanContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Commit()
    {
        _dbContext.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries().Where(r => r.State != EntityState.Detached))
        {
            entry.State = EntityState.Detached;
        }
    }
    public IBaseRepository<T> Repository<T>() where T : BaseEntity
    {
        return new BaseRepository<T>(_dbContext);
    }
    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        this.disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
public interface IUnitOfWork : IDisposable
{
    void Commit();
    Task CommitAsync();
    void Rollback();
    IBaseRepository<T> Repository<T>() where T : BaseEntity;
}