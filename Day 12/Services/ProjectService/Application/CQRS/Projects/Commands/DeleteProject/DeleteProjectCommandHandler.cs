using MediatR;
using ProjectService.Domain.Repositories;

namespace ProjectService.Application.CQRS.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {request.Id} not found");
        }

        return await _projectRepository.DeleteAsync(request.Id);
    }
}
