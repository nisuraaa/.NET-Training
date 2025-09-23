using DepartmentServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DepartmentController : ControllerBase
    {
       private readonly IDepartmentAppService _departmentService;

        public DepartmentController(IDepartmentAppService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var department = await _departmentService.AddAsync(request.Name);
            return Ok(department);
        }

        [HttpGet("{id}")]
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
