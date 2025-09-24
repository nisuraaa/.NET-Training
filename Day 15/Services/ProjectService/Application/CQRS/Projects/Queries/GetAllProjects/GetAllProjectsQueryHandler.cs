using MediatR;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;

namespace ProjectService.Application.CQRS.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<Project>>
{
    private readonly IProjectRepository _projectRepository;

    public GetAllProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<Project>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.GetAllAsync();
    }
}
