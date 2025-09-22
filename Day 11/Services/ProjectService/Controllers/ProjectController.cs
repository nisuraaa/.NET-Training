using ProjectService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProjectController : ControllerBase
    {
       private readonly IProjectAppService _projectService;

        public ProjectController(IProjectAppService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest request)
        {
            var project = await _projectService.AddAsync(request.Name);
            return Ok(project);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(string id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

    }
}
