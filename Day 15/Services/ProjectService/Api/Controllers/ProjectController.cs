using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectService.Application;
using ProjectService.Application.DTO;
using ProjectService.Domain.Entities;

namespace ProjectService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class ProjectController : ControllerBase
    {
        private readonly IProjectAppService _projectService;

        public ProjectController(IProjectAppService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOrAdmin")] // Only managers and admins can create projects
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest("Project name is required.");
                }

                var project = await _projectService.AddAsync(request.Name);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(string id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found.");
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Project>> GetProjectByName(string name)
        {
            try
            {
                var project = await _projectService.GetByNameAsync(name);
                if (project == null)
                {
                    return NotFound($"Project with name {name} not found.");
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Policy = "UserOrAbove")] // All authenticated users can view projects
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
        {
            try
            {
                var projects = await _projectService.GetAllProjects();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
