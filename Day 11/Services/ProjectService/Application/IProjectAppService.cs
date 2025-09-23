using ProjectService.Domain.Entities;

namespace ProjectService.Application;

public interface IProjectAppService
{
    Task<Project> AddAsync(string name);
    Task<Project?> GetByIdAsync(string id);
    Task<Project?> GetByNameAsync(string name);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<Project>> GetAllProjects();
}
