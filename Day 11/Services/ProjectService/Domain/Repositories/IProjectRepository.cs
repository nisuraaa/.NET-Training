namespace ProjectService.Domain.Repositories;

using ProjectService.Domain.Entities;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(string id);
    Task<Project?> GetByNameAsync(string name);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project> AddAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(string id);
}
