using MediatR;
using MassTransit;
using ProjectService.Domain.Repositories;
using SharedEvents.Events;

namespace ProjectService.Application.CQRS.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository, IPublishEndpoint publishEndpoint)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {request.Id} not found");
        }

        var result = await _projectRepository.DeleteAsync(request.Id);

        if (result)
        {
            // Publish event
            await _publishEndpoint.Publish(new ProjectDeletedEvent
            {
                ProjectId = request.Id,
                DeletedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        return result;
    }
}
