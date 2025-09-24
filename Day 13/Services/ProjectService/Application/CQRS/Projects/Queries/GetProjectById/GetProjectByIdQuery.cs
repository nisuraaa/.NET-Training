using MediatR;
using ProjectService.Domain.Entities;

namespace ProjectService.Application.CQRS.Projects.Queries.GetProjectById;

public class GetProjectByIdQuery : IRequest<Project?>
{
    public string Id { get; set; } = string.Empty;
}
