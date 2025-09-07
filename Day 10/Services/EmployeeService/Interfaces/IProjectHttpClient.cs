public interface IProjectHttpClient
{
    Task<Project?> CreateProjectAsync(string projectName);
    Task<Project?> GetProjectByNameAsync(string projectName);
    Task<Project?> GetProjectByIdAsync(string id);

}
