public interface IProjectRepository
{
    // Basic CRUD operations
    Task<Project?> GetByIdAsync(string id);
    Task<Project?> GetByNameAsync(string id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project> AddAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(string id);
}