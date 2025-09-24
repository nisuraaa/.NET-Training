using MediatR;

namespace ProjectService.Application.CQRS.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
}
