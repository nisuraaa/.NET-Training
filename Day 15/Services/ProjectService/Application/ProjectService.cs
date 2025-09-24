using ProjectService.Domain.Repositories;
using ProjectService.Domain.Entities;

namespace ProjectService.Application
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

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }
        
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> AddAsync(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException("Project name cannot be null or empty.", nameof(projectName));
            }

            var normalizedName = projectName.Trim();

            // Reuse existing project if one already exists with the same (trimmed) name
            var existing = await _projectRepository.GetByNameAsync(normalizedName);
            if (existing != null)
            {
                return existing;
            }

            var project = new Project();
            project.Initialize(Guid.NewGuid().ToString(), normalizedName);
            return await _projectRepository.AddAsync(project);
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
