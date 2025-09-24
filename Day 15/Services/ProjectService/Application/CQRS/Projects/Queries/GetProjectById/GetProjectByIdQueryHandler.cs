using MediatR;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;

namespace ProjectService.Application.CQRS.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Project?>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.GetByIdAsync(request.Id);
    }
}
