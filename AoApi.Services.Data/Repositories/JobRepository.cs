using System;
using System.Linq;
using System.Threading.Tasks;
using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AoApi.Services.Data.Repositories
{
    // <summary>
    /// Implementation of the data access layer for the Job model/table
    /// </summary>
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
        public JobRepository(AOContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a job by the attached employee id
        /// </summary>
        /// <param name="id">Id of the employee</param>
        /// <returns>the found job</returns>
        public async Task<Job> GetJobByEmployeeId(Guid id)
        {
            var foundEmployee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();

            var foundJob = await _context.Jobs.Where(j => j.Id == foundEmployee.JobId).FirstOrDefaultAsync();

            return foundJob;
        }
    }
}
