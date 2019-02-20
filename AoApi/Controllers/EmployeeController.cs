using AoApi.Services.PropertyMappingServices;
using AoApi.Services.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AoApi.Helpers;
using AoApi.Services.Data.DtoModels.EmployeeDtos;
using AoApi.Data.Models;
using AutoMapper;
using System.Collections.Generic;
using AoApi.Services;
using Newtonsoft.Json;
using System.Dynamic;
using Microsoft.AspNetCore.JsonPatch;
using Swashbuckle.AspNetCore.Annotations;

namespace AoApi.Controllers
{
    /// <summary>
    /// Controller that handles all requests to /api/employees
    /// </summary>
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IControllerHelper _controllerHelper;

        public EmployeeController(
            IEmployeeRepository employeeRepository, IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService, IControllerHelper controllerHelper)
        {
            _employeeRepository = employeeRepository;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _controllerHelper = controllerHelper;
        }

        /// <summary>
        /// Retrieves a paged list of all employees.
        /// Can also return a hateoas response if requested
        /// </summary>
        /// <param name="request">a DTO class to handle request paramters</param>
        /// <param name="mediaType">media type in case HATEAOS is requested</param>
        /// <returns>A list of paged employees</returns>
        [SwaggerOperation(
            Summary = "Retrieve all employees",
            Description = "Retrieves all the employees",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas"})]
        [SwaggerResponse(200, "All employees were returned", typeof(EmployeeDto[]))]
        [SwaggerResponse(400, "the requested field does not exist")]
        [HttpGet(Name = "GetEmployees")]
        public async Task<IActionResult> GetAllEmployeesAsync([FromQuery] RequestParameters request, [FromHeader(Name = "accept")] string mediaType)
        {
            if (string.IsNullOrWhiteSpace(request.OrderBy))
                request.OrderBy = "Name";

            if (!_typeHelperService.TypeHasProperties<EmployeeDtoForMultiple>(request.Fields))
                return BadRequest();

            var pagedEmployeeList = await _employeeRepository.GetEmployeesAsync(
                request.OrderBy, request.SearchQuery, request.PageNumber,
                request.PageSize, _propertyMappingService.GetPropertyMapping<EmployeeDtoForMultiple, Employee>());

            var paginationMetaData = _controllerHelper.CreatePaginationMetadataObject(pagedEmployeeList, request, "GetEmployees");

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaData));

            var employees = Mapper.Map<IEnumerable<EmployeeDtoForMultiple>>(pagedEmployeeList);

            var shapedEmployees = employees.ShapeData(request.Fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedAndLinkedEmployees = _controllerHelper.AddLinksToShapedObjects(shapedEmployees, "Employee", request.Fields);

                var linkedResourceCollection = _controllerHelper.AddLinksToCollection(shapedAndLinkedEmployees, request, pagedEmployeeList.HasNext, pagedEmployeeList.HasPrevious, "Employee");
                
                if (request.IncludeMetadata)
                {
                    ((IDictionary<string, object>)linkedResourceCollection).Add("metadata", paginationMetaData);
                    return Ok(linkedResourceCollection);
                }

                return Ok(linkedResourceCollection);
            }


            if (request.IncludeMetadata)
            {
                var entityWithMeaData = new ExpandoObject();
                ((IDictionary<string, object>)entityWithMeaData).Add("metadata", paginationMetaData);
                ((IDictionary<string, object>)entityWithMeaData).Add("records", shapedEmployees);
                return Ok(entityWithMeaData);
            }

            return Ok(shapedEmployees);
        }

        /// <summary>
        /// Retrieves a single employee by the ID
        /// </summary>
        /// <param name="employeeId">Id of employee</param>
        /// <param name="fields">requested properties of the object, if any</param>
        /// <param name="mediaType">media type in case HATEAOS is requested</param>
        /// <returns>An employee</returns>
        [SwaggerOperation(
            Summary = "Retrieve one employee",
            Description = "Retrieves one employee",
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(200, "Found employee returned", typeof(EmployeeDto))]
        [SwaggerResponse(400, "The requested field does not exist")]
        [SwaggerResponse(404, "No Employee was found")]
        [HttpGet("{employeeId}", Name = "GetEmployee")]
        public async Task<IActionResult> GetOneEmployeeAsync(
            [FromRoute, SwaggerParameter("Id of the employee to find", Required = true)] Guid employeeId,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var foundEmployee = await _employeeRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            if (foundEmployee == null)
                return NotFound();

            var employeeToReturn = Mapper.Map<EmployeeDto>(foundEmployee);

            if (!string.IsNullOrWhiteSpace(fields) && !_typeHelperService.TypeHasProperties<EmployeeDto>(fields))
                return BadRequest();

            var shapedEmployee = employeeToReturn.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
                ((IDictionary<string, object>)shapedEmployee).Add("links", _controllerHelper.CreateLinksForResource(foundEmployee.Id, fields, "Employee"));

            return Ok(shapedEmployee);
        }

        /// <summary>
        /// Creates an employee in the persistence layer
        /// </summary>
        /// <param name="employeeToCreate">the employee to create</param>
        /// <param name="fields">properties to return after done creating</param>
        /// <param name="mediaType">media type in case HATEAOS is requested</param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Create an employee",
            Description = "Creates an employee",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created employee returned", typeof(EmployeeDto))]
        [SwaggerResponse(500, "Failed to create an employee")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(
            [FromBody, SwaggerParameter("Employee to create", Required = true)] EmployeeCreateDto employeeToCreate,
            [FromQuery, SwaggerParameter("Request which fields you want returned")] string fields,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var employeeEntity = Mapper.Map<Employee>(employeeToCreate);

            employeeEntity.Id = Guid.NewGuid();

            _employeeRepository.Create(employeeEntity);

            if (!await _employeeRepository.SaveChangesAsync())
            {
                //  change to logging
                throw new Exception("Failed to save employee");
                //  consider to return an error to notify user of failed save
            }

            var employeeFromRepo = await _employeeRepository.GetFirstByConditionAsync(x => x.Id == employeeEntity.Id);

            var employeeToReturn = Mapper.Map<EmployeeDto>(employeeFromRepo);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                var shapedEmployee = _controllerHelper.ShapeAndAddLinkToObject(employeeToReturn, "Employee", fields);

                return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, shapedEmployee);
            }

            if (!string.IsNullOrWhiteSpace(fields))
            {
                var shapedEmployee = employeeToReturn.ShapeData(fields);

                return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, shapedEmployee);
            }

            return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, employeeToReturn);
        }

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="employeeId">Id of the employee to update</param>
        /// <param name="employeeToUpdate">What to update the employee with</param>
        /// <param name="mediaType">media type in case HATEAOS is requested</param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "update an employee",
            Description = "updates an employee, or creates one if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created employee returned", typeof(EmployeeDto))]
        [SwaggerResponse(204, "Successfully updated employee")]
        [SwaggerResponse(500, "Failed to create or update an employee")]
        [HttpPut("{employeeId}", Name = "UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployeeAsync(
            [FromRoute, SwaggerParameter("Id of the employee to update", Required = true)] Guid employeeId,
            [FromBody, SwaggerParameter("object with the updates", Required = true)]  EmployeeUpdateDto employeeToUpdate,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var employeeFromRepo = await _employeeRepository.GetFirstByConditionAsync(x => x.Id == employeeId);

            //  upserting if doesnt exist
            if (employeeFromRepo == null)
            {
                var employeeEntity = Mapper.Map<Employee>(employeeToUpdate);
                employeeEntity.Id = employeeId;

                _employeeRepository.Create(employeeEntity);

                if (!await _employeeRepository.SaveChangesAsync())
                    throw new Exception("Failed to save on upserting");

                var employeeToReturn = Mapper.Map<EmployeeDto>(await _employeeRepository.GetFirstByConditionAsync(h => h.Id == employeeId));

                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedEmployee = _controllerHelper.ShapeAndAddLinkToObject(employeeToReturn, "Employee", null);
                    return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, shapedEmployee);
                }


                return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, employeeToReturn);
            }

            Mapper.Map(employeeToUpdate, employeeFromRepo);

            _employeeRepository.Update(employeeFromRepo);

            if (!await _employeeRepository.SaveChangesAsync())
                throw new Exception("Failed to save on updating");

            return NoContent();
        }

        /// <summary>
        /// Partially update an employee
        /// </summary>
        /// <param name="employeeId">Id of the employee to partially update</param>
        /// <param name="jsonPatchDocument">The operation to perform on the employee</param>
        /// <param name="mediaType">media type in case HATEAOS is requested</param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "partually update an using jsonpatch employee",
            Description = "partially updates an employee, or creates one if none exists(upserting)",
            Consumes = new string[] { "application/json" },
            Produces = new string[] { "application/json", "application/vnd.AO.json+hateoas" })]
        [SwaggerResponse(201, "Created employee returned", typeof(EmployeeDto))]
        [SwaggerResponse(204, "Successfully updated employee")]
        [SwaggerResponse(500, "Failed to create or update an employee")]
        [HttpPatch("{employeeId}", Name = "PartiallyUpdateEmployee")]
        public async Task<IActionResult> PartiuallyUpdateEmployeeAsync(
            [FromRoute, SwaggerParameter("Id of the employee to partially update", Required = true)] Guid employeeId,
            [FromBody, SwaggerParameter("json patch operations to perform", Required = true)] JsonPatchDocument<EmployeeUpdateDto> jsonPatchDocument,
            [FromHeader(Name = "accept"), SwaggerParameter("Request Hateoas")] string mediaType)
        {
            var employeeFromRepo = await _employeeRepository.GetFirstByConditionAsync(x => x.Id == employeeId);

            //  upserting if doesnt exist
            if (employeeFromRepo == null)
            {
                var employee = new EmployeeUpdateDto();

                jsonPatchDocument.ApplyTo(employee, ModelState);

                if (!ModelState.IsValid)
                    throw new Exception("invalid model state");

                var employeeToAdd = Mapper.Map<Employee>(employee);

                employeeToAdd.Id = employeeId;

                _employeeRepository.Create(employeeToAdd);


                if (!await _employeeRepository.SaveChangesAsync())
                    throw new Exception("Failed to save on upserting");

                var employeeToReturn = Mapper.Map<EmployeeDto>(await _employeeRepository.GetFirstByConditionAsync(k => k.Id == employeeId));


                if (mediaType == "application/vnd.AO.json+hateoas")
                {
                    var shapedEmployee = _controllerHelper.ShapeAndAddLinkToObject(employeeToReturn, "Employee", null);
                    return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, shapedEmployee);
                }

                return CreatedAtRoute("GetEmployee", new { employeeId = employeeToReturn.Id }, employeeToReturn);
            }

            var employeeToPatch = Mapper.Map<EmployeeUpdateDto>(employeeFromRepo);

            jsonPatchDocument.ApplyTo(employeeToPatch, ModelState);

            if (!ModelState.IsValid)
                throw new Exception("invalid model state");

            Mapper.Map(employeeToPatch, employeeFromRepo);

            _employeeRepository.Update(employeeFromRepo);

            if (!await _employeeRepository.SaveChangesAsync())
                throw new Exception("Failed to save on updating");

            return NoContent();
        }

        /// <summary>
        /// Delete an employee 
        /// </summary>
        /// <param name="employeeId">Id of the employee to delete</param>
        /// <returns>status 200</returns>
        [SwaggerOperation(
            Summary = "Delete an existing employee",
            Description = "Deletes an existing employee")]
        [SwaggerResponse(200, "Successfully deleted an employee")]
        [SwaggerResponse(404, "Employee not found")]
        [HttpDelete("{employeeId}", Name = "DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute, SwaggerParameter("Id of employee to delete", Required = true)] Guid employeeId)
        {
            var foundEmployee = await _employeeRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            if (foundEmployee == null)
                return NotFound();

            _employeeRepository.Delete(foundEmployee);

            if (!await _employeeRepository.SaveChangesAsync())
                throw new Exception("Failed to save on deleting");

            return Ok();
        }
    }
}
