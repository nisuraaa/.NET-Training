using MediatR;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;

namespace ProjectService.Application.CQRS.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Project>
{
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var existingProject = await _projectRepository.GetByIdAsync(request.Id);
        if (existingProject == null)
        {
            throw new KeyNotFoundException($"Project with ID {request.Id} not found");
        }

        existingProject.SetName(request.Name);
        return await _projectRepository.UpdateAsync(existingProject);
    }
}
