using MediatR;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;

namespace ProjectService.Application.CQRS.Projects.Queries.GetProjectByName;

public class GetProjectByNameQueryHandler : IRequestHandler<GetProjectByNameQuery, Project?>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByNameQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project?> Handle(GetProjectByNameQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.GetByNameAsync(request.Name);
    }
}
