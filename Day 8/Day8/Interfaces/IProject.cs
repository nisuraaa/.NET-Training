public interface IProject
{
    Task<Project?> GetProjectByNameAsync(string projectName);
    Task<Project> CreateProjectAsync(string projectName);
    Task<Project> GetOrCreateProjectAsync(string projectName);
    Task<List<Project>> GetAllProjectsAsync();
}