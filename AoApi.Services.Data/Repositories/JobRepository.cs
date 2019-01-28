using System;
using System.Linq;
using System.Threading.Tasks;
using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AoApi.Services.Data.Repositories
{
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
        public JobRepository(AOContext context) : base(context)
        {
        }

        public async Task<Job> GetJobByEmployeeId(Guid id)
        {
            var foundEmployee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();

            var foundJob = await _context.Jobs.Where(j => j.Id == foundEmployee.JobId).FirstOrDefaultAsync();

            return foundJob;
        }
    }
}
