using AoApi.Services.PropertyMappingServices;
using AoApi.Services.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AoApi.Helpers;

namespace AoApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ITypeHelperService _typeHelperService;

        public EmployeeController(
            IEmployeeRepository employeeRepository, IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _employeeRepository = employeeRepository;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync(RequestParameters request)
        {
            var foundEmployees = await _employeeRepository.GetAllAsync();

            return Ok(foundEmployees);
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
