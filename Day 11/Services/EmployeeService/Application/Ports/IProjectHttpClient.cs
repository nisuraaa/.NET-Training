public interface IProjectHttpClient
{
    Task<CreateProjectResponse?> CreateProjectAsync(string projectName);
    Task<GetProjectResponse?> GetProjectByNameAsync(string projectName);
    Task<GetProjectResponse?> GetProjectByIdAsync(string id);

}
