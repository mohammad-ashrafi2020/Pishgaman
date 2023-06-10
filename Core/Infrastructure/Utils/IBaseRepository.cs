using System.Linq.Expressions;

namespace Infrastructure.Utils;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetAsync(int id);

    Task<T?> GetTracking(int id);

    void Add(T entity);

    Task AddRange(ICollection<T> entities);

    void Update(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);

    bool Exists(Expression<Func<T, bool>> expression);

    T? Get(int id);
    void Delete(T entity);

    /// <summary>
    /// Inke Tooye Repository IQueryable Bargardoonim Kare Khoobi Nist Vali Tooye In Proje Ke Dadin Majboor boodam
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryable<T> Query(Expression<Func<T, bool>> expression);

}