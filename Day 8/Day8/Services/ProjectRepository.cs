using Microsoft.EntityFrameworkCore;

public class ProjectRepository : IProjectRepository
{
    private readonly dbContext _dbContext;

    public ProjectRepository(dbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project?> GetByNameAsync(string name)
    {
        return await _dbContext.Projects
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<Project?> GetByIdAsync(string id)
    {
        return await _dbContext.Projects
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _dbContext.Projects
            .Include(p => p.Employees)
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

    public async Task<bool> DeleteAsync(string name)
    {
        var project = await GetByNameAsync(name);
        if (project == null)
        {
            return false;
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}