using AoApi.Data.Models;
using AoApi.Services.Data.DtoModels.JobDtos;
using AoApi.Services.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }


        // make is possible to have more than one job??
        [HttpGet]
        public async Task<IActionResult> GetAllJobsAsync([FromRoute] Guid employeeId)
        {
            if (!await _jobRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundJob = await _jobRepository.GetJobByEmployeeId(employeeId); // make get ALL jobs

            var jobToReturn = Mapper.Map<JobDto>(foundJob);

            return Ok(jobToReturn);
        }

        //[HttpGet("{jobId}")]
        //public async Task<IActionResult> GetOneJobAsync([FromRoute] Guid employeeId,[FromRoute] Guid jobId)
        //{
        //    if (!await _jobRepository.EntityExists<Employee>(e => e.Id == employeeId))
        //    {
        //        return NotFound();
        //    }

        //    var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

        //    if (foundJob == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(foundJob);
        //}
    }
}
