using AoApi.Data.Models;
using AoApi.Helpers;
using AoApi.Services;
using AoApi.Services.Data.DtoModels.JobDtos;
using AoApi.Services.Data.Repositories;
using AoApi.Services.PropertyMappingServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/job")]
    [ApiController]
    public class EmployeeJobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IControllerHelper _controllerHelper;

        public EmployeeJobController(IJobRepository jobRepository, IEmployeeRepository employeeRepository,
                                     ITypeHelperService typeHelperService, IControllerHelper controllerHelper)
        {
            _jobRepository = jobRepository;
            _employeeRepository = employeeRepository;
            _typeHelperService = typeHelperService;
            _controllerHelper = controllerHelper;
        }

        [HttpGet(Name = "GetEmplooyeJob")]
        public async Task<IActionResult> GetAllEmployeeJobsAsync([FromRoute] Guid employeeId, [FromQuery] string fields,
                                                                 [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrWhiteSpace(fields))
            {
                if (!_typeHelperService.TypeHasProperties<JobDto>(fields))
                    return BadRequest();
            }

            if (!await _jobRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundEmployeeJob = await _jobRepository.GetJobByEmployeeId(employeeId);

            var emplooyeJob = Mapper.Map<JobDto>(foundEmployeeJob);

            var shapedEmployeeJob = emplooyeJob.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                ((IDictionary<string, object>)shapedEmployeeJob)
                    .Add("links", _controllerHelper.CreateLinksForResource(foundEmployeeJob.Id, fields, "EmployeeJob"));
            }

            return Ok(shapedEmployeeJob);
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

        //    var jobToReturn = Mapper.Map<JobDto>(foundJob);

        //    return Ok(jobToReturn);
        //}

        [HttpPost(Name = "PostEmployeeJob")]                                                       // change to what?
        public async Task<IActionResult> PostEmployeeJobAsync([FromRoute] Guid employeeId, [FromBody] JobDto jobToGive,
                                                              [FromHeader(Name = "Accept")] string mediaType, [FromQuery] string fields)
        {
            if (!await _jobRepository.EntityExists<Employee>(e => e.Id == employeeId))
                return NotFound();

            if (!await _jobRepository.EntityExists<Job>(j => j.Id == jobToGive.Id))
                return NotFound();

            var foundEmployee = _jobRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            var mappedEmployee = Mapper.Map<Employee>(foundEmployee);

            mappedEmployee.JobId = jobToGive.Id;

            _employeeRepository.Update(mappedEmployee);

            if (!await _employeeRepository.SaveChangesAsync())
            {
                throw new Exception("Failed to save new employee job");
            }

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedEmployeeJob = _controllerHelper.ShapeAndAddLinkToObject(foundEmployee, "EmployeeJob", fields);

                return CreatedAtRoute("GetEmplooyeJob", new { employeeId = foundEmployee.Id }, shapedEmployeeJob);
            }

            if (!string.IsNullOrWhiteSpace(fields))
            {
                var shapedEmployeeJob = foundEmployee.ShapeData(fields);

                return CreatedAtRoute("GetEmplooyeJob", new { employeeId = foundEmployee.Id }, shapedEmployeeJob);
            }

            return CreatedAtRoute("GetEmplooyeJob", new { employeeId = foundEmployee.Id }, foundEmployee);

        }

        [HttpPut(Name = "PartiallyUpdateEmployeeJob")]                                               // change to what?
        public async Task<IActionResult> UpdateEmployeeJobAsync([FromRoute] Guid employeeId, [FromBody] JobDto jobToChange)
        {
            if (!await _jobRepository.EntityExists<Employee>(e => e.Id == employeeId))
                return NotFound();

            if (!await _jobRepository.EntityExists<Job>(j => j.Id == jobToChange.Id))
                return NotFound();

            var foundEmployee = _jobRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            var mappedEmployee = Mapper.Map<Employee>(foundEmployee);

            mappedEmployee.JobId = jobToChange.Id;

            _employeeRepository.Update(mappedEmployee);

            if (!await _employeeRepository.SaveChangesAsync())
            {
                throw new Exception("Failed to update employee job");
            }

            return NoContent();
        }
    }
}
