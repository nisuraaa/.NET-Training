using MediatR;
using MassTransit;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;
using SharedEvents.Events;

namespace ProjectService.Application.CQRS.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Project>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IPublishEndpoint publishEndpoint)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Project> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var existingProject = await _projectRepository.GetByIdAsync(request.Id);
        if (existingProject == null)
        {
            throw new KeyNotFoundException($"Project with ID {request.Id} not found");
        }

        existingProject.SetName(request.Name);
        var updatedProject = await _projectRepository.UpdateAsync(existingProject);

        // Publish event
        await _publishEndpoint.Publish(new ProjectUpdatedEvent
        {
            ProjectId = updatedProject.Id,
            Name = updatedProject.Name,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);

        return updatedProject;
    }
}
