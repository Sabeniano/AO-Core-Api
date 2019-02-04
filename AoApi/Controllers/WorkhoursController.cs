using AoApi.Data.Models;
using AoApi.Services.Data.DtoModels.WorkhoursDtos;
using AoApi.Services.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/workhours")]
    [ApiController]
    public class WorkhoursController : ControllerBase
    {
        private readonly IWorkhoursRepository _workhoursRepository;

        public WorkhoursController(IWorkhoursRepository workhoursRepository)
        {
            _workhoursRepository = workhoursRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkhourssAsync([FromRoute] Guid employeeId)
        {
            if (!await _workhoursRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundWorkhours = await _workhoursRepository.GetAllByConditionAsync(s => s.EmployeeId == employeeId);

            var workhoursToReturn = Mapper.Map<IEnumerable<WorkhoursDto>>(foundWorkhours);

            return Ok(workhoursToReturn);
        }

        [HttpGet("{workhoursId}")]
        public async Task<IActionResult> GetOneWorkhoursAsync([FromRoute] Guid employeeId, [FromRoute] Guid workhoursId)
        {
            if (!await _workhoursRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
            {
                return NotFound();
            }

            var workhoursToReturn = Mapper.Map<WorkhoursDto>(foundWorkhours);

            return Ok(workhoursToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkhoursAsync([FromBody] WorkhoursCreateDto workhoursToCreate)
        {
            var workhoursToAdd = Mapper.Map<Workhours>(workhoursToCreate);
            workhoursToAdd.Id = Guid.NewGuid();

            _workhoursRepository.Create(workhoursToAdd);

            await _workhoursRepository.SaveChangesAsync();

            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursToAdd.Id);

            return Ok();
        }

        [HttpPut("{workhoursId}")]
        public async Task<IActionResult> UpdateWorkhoursAsync([FromRoute] Guid workhoursId, [FromBody] WorkhoursUpdateDto workhoursToUpdate)
        {
            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
            {
                var workhoursToAdd = Mapper.Map<Workhours>(workhoursToUpdate);
                workhoursToAdd.Id = Guid.NewGuid();

                _workhoursRepository.Create(workhoursToAdd);

                await _workhoursRepository.SaveChangesAsync();

                var workhoursToReturn = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursToAdd.Id);

                return CreatedAtRoute("GetWorkhours", new { workhoursId = workhoursToReturn.Id }, workhoursToReturn);
            }

            Mapper.Map(workhoursToUpdate, foundWorkhours);

            _workhoursRepository.Update(foundWorkhours);

            await _workhoursRepository.SaveChangesAsync();

            // check if it returns "old" workhours before update. Else find and return?
            return Ok(foundWorkhours);

            //return NoContent();
        }

        [HttpPatch("{workhoursId}")]
        public async Task<IActionResult> PartialUpdateWorkhoursAsync([FromRoute] Guid workhoursId, [FromBody] JsonPatchDocument<WorkhoursUpdateDto> workhoursToPartialUpdate)
        {
            if (workhoursToPartialUpdate == null)
                return BadRequest();

            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
            {
                var workhoursToCreate = new WorkhoursUpdateDto();

                // check if necessary
                workhoursToPartialUpdate.ApplyTo(workhoursToCreate, ModelState);

                var workhoursToAdd = Mapper.Map<Workhours>(workhoursToCreate);
                workhoursToAdd.Id = Guid.NewGuid();

                _workhoursRepository.Create(workhoursToAdd);

                await _workhoursRepository.SaveChangesAsync();

                var workhoursToReturn = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursToAdd.Id);

                return CreatedAtRoute("GetWorkhours", new { workhoursId = workhoursToReturn.Id }, workhoursToReturn);
            }
            // why map back and fourth?
            var workhoursToPatch = Mapper.Map<WorkhoursUpdateDto>(foundWorkhours);

            // check if necessary
            workhoursToPartialUpdate.ApplyTo(workhoursToPatch, ModelState);

            // why map back and fourth?
            Mapper.Map(workhoursToPatch, foundWorkhours);

            _workhoursRepository.Update(foundWorkhours);

            await _workhoursRepository.SaveChangesAsync();

            // check if it returns "old" workhours before update. Else find and return?
            return Ok(foundWorkhours);

            //return NoContent();
        }

        [HttpDelete("{workhoursId}")]
        public async Task<IActionResult> DeleteWorkhoursAsync([FromRoute] Guid workhoursId)
        {
            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
                return NotFound();

            _workhoursRepository.Delete(foundWorkhours);

            await _workhoursRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
