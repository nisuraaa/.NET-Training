using DepartmentServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DepartmentController : ControllerBase
    {
       private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
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

    }
}
