using AoApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var foundEmployees = await _employeeRepository.GetAllAsync();

            return Ok(foundEmployees);
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetOneEmployeeAsync()
        {
            var foundEmployees = await _employeeRepository.GetAllAsync();

            return Ok(foundEmployees);
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
        public async Task<IActionResult> DeleteEmployeeAsync()
        {
            return Ok();
        }
    }
}
