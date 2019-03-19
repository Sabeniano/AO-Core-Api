using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AoApi.Data.Repositories
{
    /// <summary>
    /// Data access layer for models/tables
    /// </summary>
    /// <typeparam name="T">The model/table to access</typeparam>
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Asynchronously checks if an entity exists
        /// </summary>
        /// <typeparam name="TEntity">Model/Table to look in</typeparam>
        /// <param name="expression">Condition to check by</param>
        /// <returns></returns>
        Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        /// <summary>
        /// Asynchronously retrieves every record from a model/table
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Asynchronously retrieve a record based on a condition
        /// </summary>
        /// <param name="expression">Condition to retrieve by</param>
        /// <returns></returns>
        Task<T> GetFirstByConditionAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Asynchronously retrieve every record based on a condition
        /// </summary>
        /// <param name="expression">Condition to retrieve by</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Adds an entity to track in the data persistence layer
        /// </summary>
        /// <param name="entity">Entity to track</param>
        void Create(T entity);
        /// <summary>
        /// Update an entity thats being tracked in the data persistence layer
        /// </summary>
        /// <param name="entity">entity to update</param>
        void Update(T entity);
        /// <summary>
        /// Stop tracking an entity in the data persistence layer
        /// </summary>
        /// <param name="entity">Entity to Delete/Untrack</param>
        void Delete(T entity);
        /// <summary>
        /// Asynchronously save changes to the data persistence layer
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
