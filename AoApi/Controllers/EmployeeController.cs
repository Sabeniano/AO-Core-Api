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

namespace AoApi.Controllers
{
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

        [HttpGet(Name = "GetEmployees")]
        public async Task<IActionResult> GetAllEmployeesAsync([FromQuery] RequestParameters request, [FromHeader(Name = "accept")] string mediaType)
        {
            if (string.IsNullOrWhiteSpace(request.OrderBy))
            {
                request.OrderBy = "Name";
            }

            if (!_typeHelperService.TypeHasProperties<EmployeeDtoForMultiple>(request.Fields))
            {
                return BadRequest();
            }

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

        [HttpGet("{employeeId}", Name = "GetEmployee")]
        public async Task<IActionResult> GetOneEmployeeAsync([FromRoute] Guid employeeId, [FromQuery] string fields, [FromHeader(Name = "accept")] string mediaType)
        {
            var foundEmployee = await _employeeRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            if (foundEmployee == null)
                return NotFound();

            var employeeToReturn = Mapper.Map<EmployeeDto>(foundEmployee);

            if (!string.IsNullOrWhiteSpace(fields))
            {
                if (!_typeHelperService.TypeHasProperties<EmployeeDto>(fields))
                    return BadRequest();
            }

            var shapedEmployee = employeeToReturn.ShapeData(fields);

            if (mediaType == "application/vnd.AO.json+hateoas")
            {
                ((IDictionary<string, object>)shapedEmployee).Add("links", _controllerHelper.CreateLinksForResource(foundEmployee.Id, fields, "Employee"));
            }
            
            return Ok(shapedEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(EmployeeCreateDto employeeToCreate, [FromQuery] string fields, [FromHeader(Name = "accept")] string mediaType)
        {
            var employeeEntity = Mapper.Map<Employee>(employeeToCreate);

            employeeEntity.Id = Guid.NewGuid();

            _employeeRepository.Create(employeeEntity);

            if (!await _employeeRepository.SaveChangesAsync())
            {
                //  change to logging
                throw new Exception("Failed to save employee");
            }

            var employeeFromRepo = _employeeRepository.GetFirstByConditionAsync(x => x.Id == employeeEntity.Id);

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

        [HttpPut("{employeeId}", Name = "UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployeeAsync()
        {
            return Ok();
        }

        [HttpPatch("{employeeId}", Name = "PartiallyUpdateEmployee")]
        public async Task<IActionResult> PartiuallyUpdateEmployeeAsync()
        {
            return Ok();
        }

        [HttpDelete("{employeeId}", Name = "DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid employeeId)
        {
            var foundEmployee = await _employeeRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            if (foundEmployee == null)
                return NotFound();

            _employeeRepository.Delete(foundEmployee);
            return Ok();
        }
    }
}
