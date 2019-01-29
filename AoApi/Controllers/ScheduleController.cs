using AoApi.Data.Models;
using AoApi.Services.Data.DtoModels.ScheduleDtos;
using AoApi.Services.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/schedules")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }


        // make is possible to have more than one schedule??
        [HttpGet]
        public async Task<IActionResult> GetAllSchedulesAsync([FromRoute] Guid employeeId)
        {
            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundSchedule = await _scheduleRepository.GetAllByConditionAsync(s => s.EmployeeId == employeeId);

            var scheduleToReturn = Mapper.Map<IEnumerable<ScheduleDto>>(foundSchedule);

            return Ok(scheduleToReturn);
        }

        [HttpGet("{scheduleId}")]
        public async Task<IActionResult> GetOneScheduleAsync([FromRoute] Guid employeeId, [FromRoute] Guid scheduleId)
        {
            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
            {
                return NotFound();
            }

            var scheduleToReturn = Mapper.Map<ScheduleDto>(foundSchedule);

            return Ok(scheduleToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync([FromBody] ScheduleCreateDto scheduleToCreate)
        {
            var scheduleToAdd = Mapper.Map<Schedule>(scheduleToCreate);
            scheduleToAdd.Id = Guid.NewGuid();

            _scheduleRepository.Create(scheduleToAdd);

            await _scheduleRepository.SaveChangesAsync();

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleToAdd.Id);

            return Ok();
        }

        [HttpPut("{scheduleId}")]
        public async Task<IActionResult> UpdateScheduleAsync([FromRoute] Guid scheduleId, [FromBody] ScheduleUpdateDto scheduleToUpdate)
        {
            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
            {
                var scheduleToAdd = Mapper.Map<Schedule>(scheduleToUpdate);
                scheduleToAdd.Id = Guid.NewGuid();

                _scheduleRepository.Create(scheduleToAdd);

                await _scheduleRepository.SaveChangesAsync();

                var scheduleToReturn = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleToAdd.Id);

                return CreatedAtRoute("GetSchedule", new { scheduleId = scheduleToReturn.Id }, scheduleToReturn);
            }

            Mapper.Map(scheduleToUpdate, foundSchedule);

            _scheduleRepository.Update(foundSchedule);

            await _scheduleRepository.SaveChangesAsync();

            // check if it returns "old" schedule before update. Else find and return?
            return Ok(foundSchedule);

            //return NoContent();
        }

        [HttpPatch("{scheduleId}")]
        public async Task<IActionResult> PartialUpdateScheduleAsync([FromRoute] Guid scheduleId, [FromBody] JsonPatchDocument<ScheduleUpdateDto> scheduleToPartialUpdate)
        {
            if (scheduleToPartialUpdate == null)
                return BadRequest();

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
            {
                var scheduleToCreate = new ScheduleUpdateDto();

                // check if necessary
                scheduleToPartialUpdate.ApplyTo(scheduleToCreate, ModelState);

                var scheduleToAdd = Mapper.Map<Schedule>(scheduleToCreate);
                scheduleToAdd.Id = Guid.NewGuid();

                _scheduleRepository.Create(scheduleToAdd);

                await _scheduleRepository.SaveChangesAsync();

                var scheduleToReturn = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleToAdd.Id);

                return CreatedAtRoute("GetSchedule", new { scheduleId = scheduleToReturn.Id }, scheduleToReturn);
            }
            // why map back and fourth?
            var scheduleToPatch = Mapper.Map<ScheduleUpdateDto>(foundSchedule);

            // check if necessary
            scheduleToPartialUpdate.ApplyTo(scheduleToPatch, ModelState);

            // why map back and fourth?
            Mapper.Map(scheduleToPatch, foundSchedule);

            _scheduleRepository.Update(foundSchedule);

            await _scheduleRepository.SaveChangesAsync();

            // check if it returns "old" schedule before update. Else find and return?
            return Ok(foundSchedule);

            //return NoContent();
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> DeleteScheduleAsync([FromRoute] Guid scheduleId)
        {
            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
                return NotFound();

            _scheduleRepository.Delete(foundSchedule);

            await _scheduleRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
