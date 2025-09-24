using MediatR;
using ProjectService.Domain.Entities;

namespace ProjectService.Application.CQRS.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<Project>
{
    public string Name { get; set; } = string.Empty;
}
