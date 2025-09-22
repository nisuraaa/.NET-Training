
using ProjectService.Interfaces;

namespace ProjectService.Services
{
    public class ProjectService : IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Task<Project?> GetByIdAsync(string id)
        {
            return _projectRepository.GetByIdAsync(id);
        }

        public Task<Project?> GetByNameAsync(string name)
        {
            return _projectRepository.GetByNameAsync(name);
        }

        public Task<IEnumerable<Project>> GetAllAsync()
        {
            return _projectRepository.GetAllAsync();
        }

        public Task<Project> AddAsync(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException("Project name cannot be null or empty.", nameof(projectName));
            }
            var project = new Project();
            project.Initialize(Guid.NewGuid().ToString(), projectName);
            return _projectRepository.AddAsync(project);
        }

        public Task<Project> UpdateAsync(Project project)
        {
            return _projectRepository.UpdateAsync(project);
        }

        public Task<bool> DeleteAsync(string id)
        {
            return _projectRepository.DeleteAsync(id);
        }
    }
}
