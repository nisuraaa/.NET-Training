using Microsoft.EntityFrameworkCore;

public class ProjectService : IProject
{

    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project?> GetProjectByNameAsync(string projectName)
    {
        return await _projectRepository.GetByNameAsync(projectName);
    }

    public async Task<Project> CreateProjectAsync(string projectName)
    {
        var project = new Project
        {
            Id = Guid.NewGuid().ToString(),
            Name = projectName
        };

        await _projectRepository.AddAsync(project);
        return project;

    }

    public async Task<Project> GetOrCreateProjectAsync(string projectName)
    {
        // First, try to find existing project
        var existingProject = await _projectRepository.GetByNameAsync(projectName);
            
        if (existingProject != null)
        {
            return existingProject;
        }

        // If not found, create new project
        var project = new Project
        {
            Id = Guid.NewGuid().ToString(),
            Name = projectName
        };

        await _projectRepository.AddAsync(project);
        return project;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        if (projects == null)
        {
            return new List<Project>();
        }
        return projects.ToList();
    }
}