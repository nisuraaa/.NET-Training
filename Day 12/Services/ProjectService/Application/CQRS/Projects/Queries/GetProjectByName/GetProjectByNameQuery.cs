using MediatR;
using ProjectService.Domain.Entities;

namespace ProjectService.Application.CQRS.Projects.Queries.GetProjectByName;

public class GetProjectByNameQuery : IRequest<Project?>
{
    public string Name { get; set; } = string.Empty;
}
