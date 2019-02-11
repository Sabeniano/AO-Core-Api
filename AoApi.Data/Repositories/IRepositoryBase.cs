using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AoApi.Data.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<bool> EntityExists<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetFirstByConditionAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveChangesAsync();
    }
}
