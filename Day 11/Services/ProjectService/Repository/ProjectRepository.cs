using Microsoft.EntityFrameworkCore;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectDbContext _dbContext;

    public ProjectRepository(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project?> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var normalized = name.Trim();
        return await _dbContext.Projects
            .FirstOrDefaultAsync(d => d.Name == normalized);
    }

    public async Task<Project?> GetByIdAsync(string id)
    {
        return await _dbContext.Projects
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _dbContext.Projects
            .ToListAsync();
    }

    public async Task<Project> AddAsync(Project project)
    {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var project = await GetByIdAsync(id);
        if (project == null)
        {
            return false;
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}