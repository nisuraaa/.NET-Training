
namespace ProjectService.Interfaces
{
    public interface IProjectService
    {
        Task<Project?> GetByIdAsync(string id);
        Task<Project?> GetByNameAsync(string name);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> AddAsync(string projectName);
        Task<Project> UpdateAsync(Project project);
        Task<bool> DeleteAsync(string id);
    }
}
