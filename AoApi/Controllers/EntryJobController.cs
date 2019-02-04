using AoApi.Data.Models;
using AoApi.Services.Data.DtoModels.JobDtos;
using AoApi.Services.Data.Repositories;
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
    public class EntryJobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;

        public EntryJobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobsAsync()
        {
            var foundJob = await _jobRepository.GetAllAsync();

            var jobToReturn = Mapper.Map<IEnumerable<JobDto>>(foundJob);

            return Ok(jobToReturn);
        }

        [HttpGet("{jobId}", Name = "GetJob")]
        public async Task<IActionResult> GetOneJobAsync([FromRoute] Guid jobId)
        {
            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
            {
                return NotFound();
            }

            var jobToReturn = Mapper.Map<JobDto>(foundJob);

            return Ok(jobToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobAsync([FromBody] JobCreateDto jobToCreate)
        {
            var jobToAdd = Mapper.Map<Job>(jobToCreate);
            jobToAdd.Id = Guid.NewGuid();

            _jobRepository.Create(jobToAdd);

            await _jobRepository.SaveChangesAsync();

            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobToAdd.Id);

            return Ok();
        }

        [HttpPut("{jobId}")]
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

            await _jobRepository.SaveChangesAsync();

            // check if it returns "old" job before update. Else find and return?
            return Ok(foundJob);

            //return NoContent();
        }

        [HttpPatch("{jobId}")]
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

                await _jobRepository.SaveChangesAsync();

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

            await _jobRepository.SaveChangesAsync();

            // check if it returns "old" job before update. Else find and return?
            return Ok(foundJob);

            //return NoContent();
        }

        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJobAsync([FromRoute] Guid jobId)
        {
            var foundJob = await _jobRepository.GetFirstByConditionAsync(j => j.Id == jobId);

            if (foundJob == null)
                return NotFound();

            _jobRepository.Delete(foundJob);

            await _jobRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
