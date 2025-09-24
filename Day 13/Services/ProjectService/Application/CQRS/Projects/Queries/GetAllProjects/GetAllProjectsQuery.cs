using MediatR;
using ProjectService.Domain.Entities;

namespace ProjectService.Application.CQRS.Projects.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<IEnumerable<Project>>
{
}
