using MediatR;
using MassTransit;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;
using SharedEvents.Events;

namespace ProjectService.Application.CQRS.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateProjectCommandHandler(IProjectRepository projectRepository, IPublishEndpoint publishEndpoint)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project();
        project.Initialize(Guid.NewGuid().ToString(), request.Name);
        
        var createdProject = await _projectRepository.AddAsync(project);

        // Publish event
        await _publishEndpoint.Publish(new ProjectCreatedEvent
        {
            ProjectId = createdProject.Id,
            Name = createdProject.Name,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return createdProject;
    }
}
