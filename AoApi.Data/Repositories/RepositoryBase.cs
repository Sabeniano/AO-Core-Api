using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AoApi.Data.Repositories
{
    /// <summary>
    /// Generic data access layer
    /// </summary>
    /// <typeparam name="T">the model/table to access</typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AOContext _context;

        public RepositoryBase(AOContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously check if a record/entity exists
        /// </summary>
        /// <typeparam name="TEntity">the model/table to look in</typeparam>
        /// <param name="expression">the condition/expression to find by</param>
        /// <returns></returns>
        public async Task<bool> EntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            var foundEntity = await _context.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
            return foundEntity == null ? false : true;
        }

        /// <summary>
        /// Creates/tracks an entity in the data persistence layer
        /// </summary>
        /// <param name="entity">Entity to create/track</param>
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        /// <summary>
        /// Deletes/removes/stops tracking an entity from the data persistence layer
        /// </summary>
        /// <param name="entity">entity to delete/stop tracking</param>
        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Asynchronously retrieve every record in the model/table
        /// </summary>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieve one record that matches a condition from the model/table
        /// </summary>
        /// <param name="expression">Condition to retrieve by</param>
        /// <returns></returns>
        public virtual async Task<T> GetFirstByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously  retrieve very record from
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        /// <summary>
        /// Asynchronously save changes in the data persistence layer
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Update an entity in the model/table
        /// </summary>
        /// <param name="entity">entity to update</param>
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
