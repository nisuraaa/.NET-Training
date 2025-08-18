using EmployeeManagement.Interfaces;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Day9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployee _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly IProject _projectService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployee employeeService, IDepartmentService departmentService, IProject projectService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _projectService = projectService;
        _logger = logger;
    }

    // GET: api/Employee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
    {
        try
        {
            var employees = await _employeeService.GetAllEmployees();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all employees");
            return StatusCode(500, "Internal server error occurred while retrieving employees");
        }
    }

    // GET: api/Employee/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Employee ID cannot be null or empty");
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee with ID: {Id}", id);
            return StatusCode(500, "Internal server error occurred while retrieving employee");
        }
    }

    // POST: api/Employee
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee([FromBody] CreateEmployeeRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.AddEmployee(request);

            return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return StatusCode(500, "Internal server error occurred while creating employee");
        }
    }

    // PUT: api/Employee/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeRequest request)
    {
        try
        {   if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(id) || id != request.Id)
            {
                return BadRequest("Employee ID mismatch");
            }

            var updatedEmployee = await _employeeService.UpdateEmployee(request);
            return Ok(updatedEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {Id}", id);
            return StatusCode(500, "Internal server error occurred while updating employee");
        }
    }

    // DELETE: api/Employee/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Employee ID cannot be null or empty");
            }

            await _employeeService.DeleteEmployee(id);

            return NoContent(); // 204 No Content for successful deletion
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, "Employee with ID {Id} not found", id);
            return NotFound($"Employee with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {Id}", id);
            return StatusCode(500, "Internal server error occurred while deleting employee");
        }
    }

    // GET: api/Employee/department/{departmentName}
    [HttpGet("department/{departmentName}")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByDepartment(string departmentName)
    {
        try
        {
            if (string.IsNullOrEmpty(departmentName))
            {
                return BadRequest("Department name cannot be null or empty");
            }

            var allEmployees = await _employeeService.GetAllEmployees();
            var employeesByDept = allEmployees.FilterByDepartment(departmentName);

            if (!employeesByDept.Any())
            {
                return NotFound($"No employees found in department: {departmentName}");
            }

            return Ok(employeesByDept);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees by department: {DepartmentName}", departmentName);
            return StatusCode(500, "Internal server error occurred while retrieving employees by department");
        }
    }

    // GET: api/Employee/age-range
    [HttpGet("age-range")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByAgeRange(
        [FromQuery] int minAge,
        [FromQuery] int maxAge)
    {
        try
        {
            if (minAge < 0 || maxAge < 0)
            {
                return BadRequest("Age values cannot be negative");
            }

            if (minAge > maxAge)
            {
                return BadRequest("Minimum age cannot be greater than maximum age");
            }

            var employees = await _employeeService.GetAllEmployeesbyAgeRange(minAge, maxAge);

            if (!employees.Any())
            {
                return NotFound($"No employees found in age range: {minAge} - {maxAge}");
            }

            return Ok(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees by age range: {MinAge} - {MaxAge}", minAge, maxAge);
            return StatusCode(500, "Internal server error occurred while retrieving employees by age range");
        }
    }
}