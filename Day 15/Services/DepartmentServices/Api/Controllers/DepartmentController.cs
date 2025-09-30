using DepartmentServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DepartmentServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class DepartmentController : ControllerBase
    {
       private readonly IDepartmentAppService _departmentService;

        public DepartmentController(IDepartmentAppService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOrAdmin")] // Only managers and admins can create departments
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var department = await _departmentService.AddAsync(request.Name);
            return Ok(department);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAbove")] // All authenticated users can view departments
        public async Task<ActionResult<Department>> GetDepartment(string id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpGet("name/{name}")]
        [Authorize(Policy = "UserOrAbove")] // All authenticated users can view departments
        public async Task<ActionResult<Department>> GetDepartmentByName(string name)
        {
            var department = await _departmentService.GetByNameAsync(name);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
    }
}
