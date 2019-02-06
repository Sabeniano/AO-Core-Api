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
using Swashbuckle.AspNetCore.Annotations;

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

        [SwaggerOperation(
            Summary = "Retrieve all jobs",
            Description = "Retrieves all jobs",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "All jobs returned", typeof(JobDto[]))]
        [SwaggerResponse(400, "The requested field does not exist")]
        [HttpGet(Name = "GetJobs")]
        public async Task<IActionResult> GetAllJobsAsync(
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "Accept"), SwaggerParameter("Request Hateoas")] string mediaType)
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

        [SwaggerOperation(
            Summary = "Retrieve a job",
            Description = "Retrieves a job by its id",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "Job returned", typeof(JobDto))]
        [SwaggerResponse(400, "The requested field does not exist")]
        [HttpGet("{jobId}", Name = "GetJob")]
        public async Task<IActionResult> GetOneJobAsync(
            [FromRoute, SwaggerParameter("Id of the job to receive")] Guid jobId,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "Accept"), SwaggerParameter("Request Hateoas")] string mediaType)
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

        [SwaggerOperation(
            Summary = "Create a job",
            Description = "Creates a job",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created job returned", typeof(JobDto))]
        [SwaggerResponse(500, "Failed to create job")]
        [HttpPost(Name = "PostJob")]
        public async Task<IActionResult> CreateJobAsync(
            [FromBody, SwaggerParameter("Object with job to create", Required = true)] JobCreateDto jobToCreate,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "Accept"), SwaggerParameter("Request Hateoas")] string mediaType)
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

        [SwaggerOperation(
            Summary = "Update a job",
            Description = "Updates a job, or creates one if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created job returned", typeof(JobDto))]
        [SwaggerResponse(204, "Successfully updated job")]
        [SwaggerResponse(500, "Failed to create or update job")]
        [HttpPut("{jobId}", Name = "UpdateJob")]
        public async Task<IActionResult> UpdateJobAsync(
            [FromRoute, SwaggerParameter("Id of the job to update", Required = true)] Guid jobId, 
            [FromBody, SwaggerParameter("Object with updates to job", Required = true)] JobUpdateDto jobToUpdate,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
            {
                var jobToAdd = Mapper.Map<Job>(jobToUpdate);
                jobToAdd.Id = Guid.NewGuid();

                _jobRepository.Create(jobToAdd);

                await _jobRepository.SaveChangesAsync();

                var jobToReturn = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobToAdd.Id);

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedJob = _controllerHelper.ShapeAndAddLinkToObject(jobToReturn, "Job", null);
                    return CreatedAtRoute("GetJob", new { jobId = jobToReturn.Id }, shapedJob);
                }

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

        [SwaggerOperation(
            Summary = "Partially update a job using jsonpatch",
            Description = "Partially updates a job, or creates one if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created job returned", typeof(JobDto))]
        [SwaggerResponse(204, "Successfully updated job")]
        [SwaggerResponse(500, "Failed to create or update job")]
        [HttpPatch("{jobId}", Name = "PartiallyUpdateJob")]
        public async Task<IActionResult> PartialUpdateJobAsync(
            [FromRoute, SwaggerParameter("Id of the job to partially update", Required = true)] Guid jobId, 
            [FromBody, SwaggerParameter("Json patch operations to perform", Required = true)] JsonPatchDocument<JobUpdateDto> jobToPartialUpdate,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
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

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedJob = _controllerHelper.ShapeAndAddLinkToObject(jobToReturn, "Job", null);
                    return CreatedAtRoute("GetJob", new { jobId = jobToReturn.Id }, shapedJob);
                }

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

        [SwaggerOperation(
            Summary = "Delete an existing job",
            Description = "Deletes an existing job")]
        [SwaggerResponse(200, "Successfully deleted job")]
        [SwaggerResponse(404, "Job not found")]
        [SwaggerResponse(500, "Failed to delete job")]
        [HttpDelete("{jobId}", Name = "DeleteJob")]
        public async Task<IActionResult> DeleteJobAsync(
            [FromRoute, SwaggerParameter("Id of job to delete", Required = true)] Guid jobId)
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
