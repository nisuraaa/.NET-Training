using Microsoft.EntityFrameworkCore;

public class ProjectService : IProject
{
    public async Task<Project?> GetProjectByNameAsync(string projectName)
    {
        using var context = new dbContext();
        return await context.Projects
        .FirstOrDefaultAsync(p => p.Name.ToUpper() == projectName.ToUpper());
    }

    public async Task<Project> CreateProjectAsync(string projectName)
    {
        var project = new Project
        {
            Id = Guid.NewGuid().ToString(),
            Name = projectName
        };

        using var context = new dbContext();
        context.Projects.Add(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> GetOrCreateProjectAsync(string projectName)
    {
        using var context = new dbContext();
        
        // First, try to find existing project
        var existingProject = await context.Projects
            .FirstOrDefaultAsync(p => p.Name.ToUpper() == projectName.ToUpper());
            
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

        context.Projects.Add(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        using var context = new dbContext();
        return await context.Projects.ToListAsync();
    }
}