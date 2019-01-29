using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AoApi.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AOContext _context;

        public RepositoryBase(AOContext context)
        {
            _context = context;
        }

        public async Task<bool> EmployeeExists(Guid id)
        {
            var foundEmployee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            return foundEmployee == null ? false : true;
        }

        public async Task<bool> EntityExists<T>(Expression<Func<T, bool>> expression) where T : class
        {
            var foundEntity = await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
            return foundEntity == null ? false : true;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetFirstByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
