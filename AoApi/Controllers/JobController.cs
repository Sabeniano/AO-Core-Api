using AoApi.Data.Models;
using AoApi.Helpers;
using AoApi.Services;
using AoApi.Services.Data.DtoModels.JobDtos;
using AoApi.Services.Data.Repositories;
using AoApi.Services.PropertyMappingServices;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IControllerHelper _controllerHelper;

        public JobController(IJobRepository jobRepository, ITypeHelperService typeHelperService,
                             IControllerHelper controllerHelper)
        {
            _jobRepository = jobRepository;
            _typeHelperService = typeHelperService;
            _controllerHelper = controllerHelper;
        }

        [HttpGet(Name = "GetJobs")]
        public async Task<IActionResult> GetAllJobsAsync([FromQuery] string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrWhiteSpace(fields))
            {
                if (!_typeHelperService.TypeHasProperties<JobDto>(fields))
                    return BadRequest();
            }

            var foundJobs = await _jobRepository.GetAllAsync();

            var jobs = Mapper.Map<IEnumerable<JobDto>>(foundJobs);

            var shapedJobs = jobs.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedAndLinkedJobs = _controllerHelper.AddLinksToShapedObjects(shapedJobs, "Job", fields);

                var linkedResourceCollection = _controllerHelper.AddLinksToCollection(shapedAndLinkedJobs, "Job", null);

                return Ok(linkedResourceCollection);
            }

            return Ok(shapedJobs);
        }

        [HttpGet("{jobId}", Name = "GetJob")]
        public async Task<IActionResult> GetOneJobAsync([FromRoute] Guid jobId, [FromQuery] string fields,
                                                        [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!string.IsNullOrWhiteSpace(fields))
            {
                if (!_typeHelperService.TypeHasProperties<JobDto>(fields))
                    return BadRequest();
            }

            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
            {
                return NotFound();
            }

            var job = Mapper.Map<JobDto>(foundJob);

            var shapedEmployeeJob = job.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                ((IDictionary<string, object>)shapedEmployeeJob)
                    .Add("links", _controllerHelper.CreateLinksForResource(foundJob.Id, fields, "Job"));
            }

            return Ok(shapedEmployeeJob);
        }

        [HttpPost(Name = "PostJob")]
        public async Task<IActionResult> CreateJobAsync([FromBody] JobCreateDto jobToCreate, [FromQuery] string fields,
                                                        [FromHeader(Name = "Accept")] string mediaType)
        {
            var jobToAdd = Mapper.Map<Job>(jobToCreate);
            jobToAdd.Id = Guid.NewGuid();

            _jobRepository.Create(jobToAdd);

            if (!await _jobRepository.SaveChangesAsync())
            {
                throw new Exception("Failed to save new job");
            }

            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobToAdd.Id);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedJob = _controllerHelper.ShapeAndAddLinkToObject(foundJob, "Job", fields);

                return CreatedAtRoute("GetJob", new { jobId = foundJob.Id }, shapedJob);
            }

            if (!string.IsNullOrWhiteSpace(fields))
            {
                var shapedJob = foundJob.ShapeData(fields);

                return CreatedAtRoute("GetJob", new { jobId = foundJob.Id }, shapedJob);
            }

            return CreatedAtRoute("GetJob", new { jobId = foundJob.Id }, foundJob);

        }

        [HttpPut("{jobId}", Name = "UpdateJob")]
        public async Task<IActionResult> UpdateJobAsync([FromRoute] Guid jobId, [FromBody] JobUpdateDto jobToUpdate)
        {
            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
            {
                var jobToAdd = Mapper.Map<Job>(jobToUpdate);
                jobToAdd.Id = Guid.NewGuid();

                _jobRepository.Create(jobToAdd);

                await _jobRepository.SaveChangesAsync();

                var jobToReturn = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobToAdd.Id);

                return CreatedAtRoute("GetJob", new { jobId = jobToReturn.Id }, jobToReturn);
            }

            Mapper.Map(jobToUpdate, foundJob);

            _jobRepository.Update(foundJob);

            if (!await _jobRepository.SaveChangesAsync())
            {
                throw new Exception("Failed to update job");
            }

            return NoContent();
        }

        [HttpPatch("{jobId}", Name = "PartiallyUpdateJob")]
        public async Task<IActionResult> PartialUpdateJobAsync([FromRoute] Guid jobId, [FromBody] JsonPatchDocument<JobUpdateDto> jobToPartialUpdate)
        {
            if (jobToPartialUpdate == null)
                return BadRequest();

            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
            {
                var jobToCreate = new JobUpdateDto();

                // check if necessary
                jobToPartialUpdate.ApplyTo(jobToCreate, ModelState);

                var jobToAdd = Mapper.Map<Job>(jobToCreate);
                jobToAdd.Id = Guid.NewGuid();

                _jobRepository.Create(jobToAdd);

                if (!await _jobRepository.SaveChangesAsync())
                {
                    throw new Exception("Failed to partially update job");
                }

                var jobToReturn = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobToAdd.Id);

                return CreatedAtRoute("GetJob", new { jobId = jobToReturn.Id }, jobToReturn);
            }
            // why map back and fourth?
            var jobToPatch = Mapper.Map<JobUpdateDto>(foundJob);

            // check if necessary
            jobToPartialUpdate.ApplyTo(jobToPatch, ModelState);

            // why map back and fourth?
            Mapper.Map(jobToPatch, foundJob);

            _jobRepository.Update(foundJob);

            if (!await _jobRepository.SaveChangesAsync())
            {
                throw new Exception("Failed to partially update job");
            }


            return NoContent();
        }

        [HttpDelete("{jobId}", Name = "DeleteJob")]
        public async Task<IActionResult> DeleteJobAsync([FromRoute] Guid jobId)
        {
            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
                return NotFound();

            _jobRepository.Delete(foundJob);

            if (!await _jobRepository.SaveChangesAsync())
            {
                throw new Exception("Failed to delete job");
            }

            return Ok();
        }
    }
}
