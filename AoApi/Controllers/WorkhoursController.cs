using AoApi.Data.Models;
using AoApi.Services;
using AoApi.Services.Data.DtoModels.WorkhoursDtos;
using AoApi.Services.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using AoApi.Helpers;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/workhours")]
    [ApiController]
    public class WorkhoursController : ControllerBase
    {
        private readonly IWorkhoursRepository _workhoursRepository;
        private readonly IControllerHelper _controllerHelper;

        public WorkhoursController(
            IWorkhoursRepository workhoursRepository,
            IControllerHelper controllerHelper)
        {
            _workhoursRepository = workhoursRepository;
            _controllerHelper = controllerHelper;
        }

        [SwaggerOperation(
            Summary = "Retrieve all workhours",
            Description = "Retrieves all the workhours of one employee",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "All workhours were returned", typeof(WorkhoursDto[]))]
        [SwaggerResponse(400, "The requested field does not exist")]
        [HttpGet(Name = "GetWorkhours")]
        public async Task<IActionResult> GetAllWorkhourssAsync(
            [FromRoute] Guid employeeId,
            [FromQuery] RequestParameters request,
            [FromHeader(Name = "accept")] string mediaType)
        {
            if (!await _workhoursRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundWorkhours = await _workhoursRepository.GetAllByConditionAsync(s => s.EmployeeId == employeeId);

            var workhoursToReturn = Mapper.Map<IEnumerable<WorkhoursDto>>(foundWorkhours);

            var shapedWorkhours = workhoursToReturn.ShapeData(request.Fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedAndLinkedWorkhours = _controllerHelper.AddLinksToShapedObjects(shapedWorkhours, "Workhours", request.Fields);

                var linkedResourceCollection = _controllerHelper.AddLinksToCollection(shapedAndLinkedWorkhours, request, false, false, "Workhours");

                return Ok(linkedResourceCollection);
            }
            return Ok(shapedWorkhours);
        }

        [SwaggerOperation(
            Summary = "Retrieve one workhour",
            Description = "Retrieves workhours of one employee",
            Produces = new string[] { "application/jason", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "Found workhour returned", typeof(WorkhoursDto))]
        [SwaggerResponse(400, "The requested field does not exist")]
        [SwaggerResponse(404, "No Workhour was found")]
        [HttpGet("{workhoursId}", Name = "GetWorkhour")]
        public async Task<IActionResult> GetOneWorkhoursAsync(
            [FromRoute, SwaggerParameter("Id of employee to find", Required = true)] Guid employeeId,
            [FromRoute, SwaggerParameter("Id of workhours to find", Required = true)] Guid workhoursId,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
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

            var shapedWorkhours = workhoursToReturn.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
                ((IDictionary<string, object>)shapedWorkhours).Add("links", _controllerHelper.CreateLinksForResource(foundWorkhours.Id, fields, "Workhours"));

            return Ok(shapedWorkhours);
        }
        [SwaggerOperation(
            Summary = "Create workhours",
            Description = "Creates workhours for employee",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created workhours returned", typeof(WorkhoursDto))]
        [SwaggerResponse(500, "Failed to create workhours")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkhoursAsync(
            [FromBody, SwaggerParameter("Workhours to create", Required = true)] WorkhoursCreateDto workhoursToCreate,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var workhoursToAdd = Mapper.Map<Workhours>(workhoursToCreate);

            workhoursToAdd.Id = Guid.NewGuid();

            _workhoursRepository.Create(workhoursToAdd);

            if (!await _workhoursRepository.SaveChangesAsync())
            {
                //  change to logging
                throw new Exception("Failed to save workhours");
                //  consider to return an error to notify user of failed save
            }

            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursToAdd.Id);

            var workhoursToReturn = Mapper.Map<WorkhoursDto>(foundWorkhours);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedWorkhours = _controllerHelper.ShapeAndAddLinkToObject(workhoursToReturn, "Workhours", fields);

                return CreatedAtRoute("GetWorkhour", new { workhoursId = workhoursToReturn.Id }, shapedWorkhours);
            }

            if (!string.IsNullOrWhiteSpace(fields))
            {
                var shapedWorkhours = workhoursToReturn.ShapeData(fields);

                return CreatedAtRoute("GetWorkhour", new { workhoursId = workhoursToReturn.Id }, shapedWorkhours);
            }

            return CreatedAtRoute("GetWorkhour", new { workhoursId = workhoursToReturn.Id }, workhoursToReturn);
        }

        [SwaggerOperation(
            Summary = "Update workhours",
            Description = "Updates workhours for employee, or creates if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created workhours returned", typeof(WorkhoursDto))]
        [SwaggerResponse(204, "Succesfully updated workhours")]
        [SwaggerResponse(500, "Failed to create or update workhours")]
        [HttpPut("{workhoursId}", Name = "UpdateWorkhours")]
        public async Task<IActionResult> UpdateWorkhoursAsync(
            [FromRoute, SwaggerParameter("Id of workhours to update", Required = true)] Guid workhoursId,
            [FromBody, SwaggerParameter("Object with the updates", Required = true)] WorkhoursUpdateDto workhoursToUpdate,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
            {
                var workhoursToAdd = Mapper.Map<Workhours>(workhoursToUpdate);
                workhoursToAdd.Id = Guid.NewGuid();

                _workhoursRepository.Create(workhoursToAdd);

                if (!await _workhoursRepository.SaveChangesAsync())
                    throw new Exception("Failed to save an upserting");

                var workhoursToReturn = Mapper.Map<WorkhoursDto>(await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId));

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedWorkhours = _controllerHelper.ShapeAndAddLinkToObject(workhoursToReturn, "Workhours", null);

                    return CreatedAtRoute("GetWorkhours", new { workhoursId = workhoursToReturn.Id }, shapedWorkhours);
                }
                return CreatedAtRoute("GetWorkhours", new { workhoursId = workhoursToReturn.Id }, workhoursToReturn);
            }

            Mapper.Map(workhoursToUpdate, foundWorkhours);

            _workhoursRepository.Update(foundWorkhours);

            if (!await _workhoursRepository.SaveChangesAsync())
                throw new Exception("Failed to save on upserting");

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Partially update using jsonpatch",
            Description = "Partially updates workhours for employee, or creates if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created workhours returned", typeof(WorkhoursDto))]
        [SwaggerResponse(204, "Succesfully updated workhours")]
        [SwaggerResponse(500, "Failed to create or update workhours")]
        [HttpPatch("{workhoursId}", Name = "PartiallyUpdateWorkhours")]
        public async Task<IActionResult> PartialUpdateWorkhoursAsync(
            [FromRoute, SwaggerParameter("Id of workhours to partially update", Required = true)] Guid workhoursId,
            [FromBody, SwaggerParameter("Json patch operations to perform", Required = true)] JsonPatchDocument<WorkhoursUpdateDto> jsonPatchDocument,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
            {
                var workhoursToCreate = new WorkhoursUpdateDto();

                jsonPatchDocument.ApplyTo(workhoursToCreate, ModelState);

                if (!ModelState.IsValid)
                    throw new Exception("Invalid model state");

                var workhoursToAdd = Mapper.Map<Workhours>(workhoursToCreate);

                workhoursToAdd.Id = Guid.NewGuid();

                _workhoursRepository.Create(workhoursToAdd);

                if (!await _workhoursRepository.SaveChangesAsync())
                    throw new Exception("Failed to save on upserting");

                //var workhoursToReturn = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursToAdd.Id);
                var workhoursToReturn = Mapper.Map<WorkhoursDto>(workhoursToAdd);

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedWorkhours = _controllerHelper.ShapeAndAddLinkToObject(workhoursToReturn, "Workhours", null);

                    return CreatedAtRoute("GetWorkhours", new { workhoursId = workhoursToReturn.Id }, shapedWorkhours);
                }
                return CreatedAtRoute("GetWorkhours", new { workhoursId = workhoursToReturn.Id }, workhoursToReturn);
            }
            // why map back and fourth?
            var workhoursToPatch = Mapper.Map<WorkhoursUpdateDto>(foundWorkhours);

            // check if necessary
            jsonPatchDocument.ApplyTo(workhoursToPatch, ModelState);

            if (!ModelState.IsValid)
                throw new Exception("Invalid model state");

            // why map back and fourth?
            Mapper.Map(workhoursToPatch, foundWorkhours);

            _workhoursRepository.Update(foundWorkhours);

            if (!await _workhoursRepository.SaveChangesAsync())
                throw new Exception("Failed to save on upserting");

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Delete existing workhours",
            Description = "Deletes existing workhours of employee")]
        [SwaggerResponse(200, "Successfully deleted workhours")]
        [SwaggerResponse(404, "Workhours not found")]
        [HttpDelete("{workhoursId}", Name = "DeleteWorkhours")]
        public async Task<IActionResult> DeleteWorkhoursAsync(
            [FromRoute, SwaggerParameter("Id of workhours to delete", Required = true)] Guid workhoursId)
        {
            var foundWorkhours = await _workhoursRepository.GetFirstByConditionAsync(j => j.Id == workhoursId);

            if (foundWorkhours == null)
                return NotFound();

            _workhoursRepository.Delete(foundWorkhours);

            if (!await _workhoursRepository.SaveChangesAsync())
                throw new Exception("Failed to save on deleting");

            return Ok();
        }
    }
}
