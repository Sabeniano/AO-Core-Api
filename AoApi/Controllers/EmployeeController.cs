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
        public async Task<IActionResult> GetAllEmployeesAsync([FromQuery] RequestParameters request, [FromHeader]string mediaType)
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

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetOneEmployeeAsync([FromRoute] Guid employeeId)
        {
            var foundEmployee = await _employeeRepository.GetFirstByConditionAsync(e => e.Id == employeeId);

            if (foundEmployee == null)
                return NotFound();

            return Ok(foundEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync()
        {
            return Ok();
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeAsync()
        {
            return Ok();
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiuallyUpdateEmployeeAsync()
        {
            return Ok();
        }

        [HttpDelete("{employeeId}")]
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
