using AoApi.Data.Models;
using AoApi.Helpers;
using AoApi.Services;
using AoApi.Services.Data.DtoModels.ScheduleDtos;
using AoApi.Services.Data.Repositories;
using AoApi.Services.PropertyMappingServices;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/schedules")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IControllerHelper _controllerHelper;

        public ScheduleController(
            IScheduleRepository scheduleRepository, ITypeHelperService typeHelperService,
            IPropertyMappingService propertyMappingService, IControllerHelper controllerHelper)
        {
            _scheduleRepository = scheduleRepository;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;
            _controllerHelper = controllerHelper;
        }


        [SwaggerOperation(
            Summary = "Retrieve all schedules belonging to an employee",
            Description = "Retrieves all schedules belonging to the employee",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "All schedules were returned", typeof(ScheduleDto[]))]
        [SwaggerResponse(400, "the requested field does not exist")]
        [SwaggerResponse(404, "the employee does not exist")]
        [HttpGet(Name = "GetSchedules")]
        public async Task<IActionResult> GetAllSchedulesAsync(
            [FromRoute, SwaggerParameter("Id of the employee to find schedules for", Required = true)] Guid employeeId,
            [FromQuery] RequestParameters request,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            if (string.IsNullOrWhiteSpace(request.OrderBy))
                request.OrderBy = "WorkDate";

            if (!_typeHelperService.TypeHasProperties<ScheduleDto>(request.Fields))
                return BadRequest();

            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var pagedSchedules = await _scheduleRepository.GetSchedulesAsync(
                request.OrderBy, request.SearchQuery, request.PageNumber,
                request.PageSize, _propertyMappingService.GetPropertyMapping<ScheduleDto, Schedule>(),
                employeeId);

            var paginationMetadata = _controllerHelper.CreatePaginationMetadataObject(pagedSchedules, request, "GetSchedules");

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var schedules = Mapper.Map<IEnumerable<ScheduleDto>>(pagedSchedules);


            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedSchedules = schedules.ShapeData(request.Fields);
                var shapedAndLinkedSchedules = _controllerHelper.AddLinksToShapedObjects(shapedSchedules, "Schedule", request.Fields);

                var linkedResourceCollection = _controllerHelper.AddLinksToCollection(shapedAndLinkedSchedules, request, pagedSchedules.HasNext, pagedSchedules.HasPrevious, "Schedule");

                if (request.IncludeMetadata)
                {
                    ((IDictionary<string, object>)linkedResourceCollection).Add("metadata", paginationMetadata);
                    return Ok(linkedResourceCollection);
                }

                return Ok(linkedResourceCollection);
            }


            if (request.IncludeMetadata)
            {
                var entityWithMeaData = new ExpandoObject();
                ((IDictionary<string, object>)entityWithMeaData).Add("metadata", paginationMetadata);

                if (!string.IsNullOrWhiteSpace(request.Fields))
                    ((IDictionary<string, object>)entityWithMeaData).Add("records", schedules.ShapeData(request.Fields));
                else
                    ((IDictionary<string, object>)entityWithMeaData).Add("records", schedules);

                return Ok(entityWithMeaData);
            }

            if (!string.IsNullOrWhiteSpace(request.Fields))
            {
                return Ok(schedules.ShapeData(request.Fields));
            }

            return Ok(schedules);
        }

        [SwaggerOperation(
            Summary = "Retrieve one schedule belonging to an employee",
            Description = "Retrieves one schedule belonging to the employee",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "All schedules were returned", typeof(ScheduleDto))]
        [SwaggerResponse(400, "the requested field does not exist")]
        [SwaggerResponse(404, "the employee or schedule does not exist")]
        [HttpGet("{scheduleId}", Name = "GetSchedule")]
        public async Task<IActionResult> GetOneScheduleAsync(
            [FromRoute, SwaggerParameter("Id of the employee to find schedule of", Required = true)] Guid employeeId,
            [FromRoute, SwaggerParameter("Id of the schedule to find", Required = true)] Guid scheduleId,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            if (!string.IsNullOrWhiteSpace(fields) && !_typeHelperService.TypeHasProperties<ScheduleDto>(fields))
                return BadRequest();

            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
            {
                return NotFound();
            }

            var mappedSchedule = Mapper.Map<ScheduleDto>(foundSchedule);

            var shapedSchedule = mappedSchedule.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
                ((IDictionary<string, object>)shapedSchedule).Add("links", _controllerHelper.CreateLinksForResource(foundSchedule.Id, fields, "Schedule"));

            return Ok(shapedSchedule);
        }

        [SwaggerOperation(
            Summary = "Create a schedule for an employee",
            Description = "Creates a schedule for an employee",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Successfully created schedule returned", typeof(ScheduleDto))]
        [SwaggerResponse(400, "the requested field does not exist")]
        [SwaggerResponse(404, "the employee does not exist")]
        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync(
            [FromRoute, SwaggerParameter("Id of employee to create schedule for", Required = true)] Guid employeeId,
            [FromBody, SwaggerParameter("Schedule object to create", Required = true)] ScheduleCreateDto scheduleToCreate,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
                return NotFound();

            var scheduleToAdd = Mapper.Map<Schedule>(scheduleToCreate);
            scheduleToAdd.Id = Guid.NewGuid();
            scheduleToAdd.EmployeeId = employeeId;

            _scheduleRepository.Create(scheduleToAdd);

            if (!await _scheduleRepository.SaveChangesAsync())
                throw new Exception("Failed to save on creating");

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleToAdd.Id);
            var scheduleToreturn = Mapper.Map<ScheduleDto>(foundSchedule);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedSchedule = _controllerHelper.ShapeAndAddLinkToObject(scheduleToreturn, "Schedule", fields);

                return CreatedAtRoute("GetSchedule", new { employeeId, scheduleId = scheduleToreturn.Id }, shapedSchedule);
            }

            if (!string.IsNullOrWhiteSpace(fields))
            {
                var shapedSchedule = scheduleToreturn.ShapeData(fields);

                return CreatedAtRoute("GetSchedule", new { employeeId, scheduleId = scheduleToreturn.Id }, shapedSchedule);
            }

            return CreatedAtRoute("GetEmployee", new { employeeId, scheduleId = scheduleToreturn.Id }, scheduleToreturn);
        }

        [SwaggerOperation(
            Summary = "update a schedule for an employee",
            Description = "updates a schedule belonging to an employee, or creates one if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Successfully created schedule returned", typeof(ScheduleDto))]
        [SwaggerResponse(204, "Successfully updated schedule")]
        [SwaggerResponse(404, "employee to update schedule for not found")]
        [SwaggerResponse(500, "Failed to create or update an employee")]
        [HttpPut("{scheduleId}", Name = "UpdateSchedule")]
        public async Task<IActionResult> UpdateScheduleAsync(
            [FromRoute, SwaggerParameter("Id of employee to update schedule for", Required = true)] Guid employeeId,
            [FromRoute, SwaggerParameter("Id of schedule to update", Required = true)] Guid scheduleId,
            [FromBody, SwaggerParameter("schedule object to update with", Required = true)] ScheduleUpdateDto scheduleToUpdate,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
                return NotFound();

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
            {
                var scheduleToAdd = Mapper.Map<Schedule>(scheduleToUpdate);
                scheduleToAdd.Id = Guid.NewGuid();
                scheduleToAdd.EmployeeId = employeeId;

                _scheduleRepository.Create(scheduleToAdd);

                if (!await _scheduleRepository.SaveChangesAsync())
                    throw new Exception("Failed to save on upserting");

                var scheduleToReturn = Mapper.Map<ScheduleDto>(await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleToAdd.Id));

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedSchedule= _controllerHelper.ShapeAndAddLinkToObject(scheduleToReturn, "Employee", null);
                    return CreatedAtRoute("GetEmployee", new { employeeId, scheduleId = scheduleToReturn.Id }, shapedSchedule);
                }

                return CreatedAtRoute("GetSchedule", new { employeeId, scheduleId = scheduleToReturn.Id }, scheduleToReturn);
            }

            Mapper.Map(scheduleToUpdate, foundSchedule);

            _scheduleRepository.Update(foundSchedule);

            if (!await _scheduleRepository.SaveChangesAsync())
                throw new Exception("Failed to save on updating");

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "partually update a schedule belonging to an emloyee using jsonpatch",
            Description = "partially updates a schedule belonging to an employee, or creates one if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created employee returned", typeof(ScheduleDto))]
        [SwaggerResponse(204, "Successfully updated employee")]
        [SwaggerResponse(404, "employee to partially update schedule for not found")]
        [SwaggerResponse(500, "Failed to create or update the schedule")]
        [HttpPatch("{scheduleId}", Name = "PartiallyUpdateSchedule")]
        public async Task<IActionResult> PartialUpdateScheduleAsync(
            [FromRoute, SwaggerParameter("Id of employee to update schedule for", Required = true)] Guid employeeId,
            [FromRoute, SwaggerParameter("Id of schedule to update", Required = true)] Guid scheduleId,
            [FromBody, SwaggerParameter("json patch operations to perform", Required = true)] JsonPatchDocument<ScheduleUpdateDto> scheduleToPartialUpdate,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
                return NotFound();

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
            {
                var scheduleToCreate = new ScheduleUpdateDto();

                // check if necessary
                scheduleToPartialUpdate.ApplyTo(scheduleToCreate, ModelState);

                if (!ModelState.IsValid)
                    throw new Exception("invalid model state");

                var scheduleToAdd = Mapper.Map<Schedule>(scheduleToCreate);
                scheduleToAdd.Id = Guid.NewGuid();
                scheduleToAdd.EmployeeId = employeeId;

                _scheduleRepository.Create(scheduleToAdd);

                if (!await _scheduleRepository.SaveChangesAsync())
                    throw new Exception("Failed to save on upserting");

                var scheduleToReturn = Mapper.Map<ScheduleDto>(await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleToAdd.Id));

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedSchedule = _controllerHelper.ShapeAndAddLinkToObject(scheduleToReturn, "Schedule", null);
                    return CreatedAtRoute("GetEmployee", new { employeeId, scheduleId = scheduleToReturn.Id }, shapedSchedule);
                }

                return CreatedAtRoute("GetSchedule", new { employeeId, scheduleId = scheduleToReturn.Id }, scheduleToReturn);
            }


            var scheduleToPatch = Mapper.Map<ScheduleUpdateDto>(foundSchedule);

            scheduleToPartialUpdate.ApplyTo(scheduleToPatch, ModelState);

            if (!ModelState.IsValid)
                throw new Exception("invalid model state");

            Mapper.Map(scheduleToPatch, foundSchedule);

            _scheduleRepository.Update(foundSchedule);

            if (!await _scheduleRepository.SaveChangesAsync())
                throw new Exception("Failed to save on updating");

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Delete a schedule",
            Description = "Deletes an existing schedule belonging to an employee")]
        [SwaggerResponse(200, "Successfully deleted the schedule")]
        [SwaggerResponse(404, "Schedule or employee not found")]
        [HttpDelete("{scheduleId}", Name = "DeleteSchedule")]
        public async Task<IActionResult> DeleteScheduleAsync(
            [FromRoute, SwaggerParameter("Id of employee to delete schedule for", Required = true)] Guid employeeId,
            [FromRoute, SwaggerParameter("Id of schedule to delete", Required = true)] Guid scheduleId)
        {
            if (!await _scheduleRepository.EntityExists<Employee>(e => e.Id == employeeId))
                return NotFound();

            var foundSchedule = await _scheduleRepository.GetFirstByConditionAsync(j => j.Id == scheduleId);

            if (foundSchedule == null)
                return NotFound();

            _scheduleRepository.Delete(foundSchedule);

            if (!await _scheduleRepository.SaveChangesAsync())
                throw new Exception("Failed to save on deleting");

            return Ok();
        }
    }
}
