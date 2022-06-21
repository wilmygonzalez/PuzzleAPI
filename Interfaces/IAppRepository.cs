using System;
using System.Linq.Expressions;
using PuzzleAPI.Data;

namespace PuzzleAPI.Interfaces
{
	public interface IAppRepository<T> where T : AppBaseEntity
    {
        IQueryable<T> GetAll();
        Task InsertAsync(T entity);
        Task InsertAsync(IEnumerable<T> entity);
        Task UpdateAsync(T entity);
        Task UpdateAsync(IEnumerable<T> entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entity);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<bool> Exists(int id);
        Task<bool> Exists(Expression<Func<T, bool>> expression);
    }
}

