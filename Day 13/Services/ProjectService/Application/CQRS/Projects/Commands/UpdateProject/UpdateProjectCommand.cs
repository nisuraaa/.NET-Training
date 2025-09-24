using MediatR;
using ProjectService.Domain.Entities;

namespace ProjectService.Application.CQRS.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<Project>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
