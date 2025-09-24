using MediatR;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Repositories;
using ProjectService.Application.CQRS.Projects.Commands.CreateProject;
using ProjectService.Application.CQRS.Projects.Commands.UpdateProject;
using ProjectService.Application.CQRS.Projects.Commands.DeleteProject;
using ProjectService.Application.CQRS.Projects.Queries.GetProjectById;
using ProjectService.Application.CQRS.Projects.Queries.GetProjectByName;
using ProjectService.Application.CQRS.Projects.Queries.GetAllProjects;

namespace ProjectService.Application
{
    public class ProjectAppService : IProjectAppService
{
    private readonly IMediator _mediator;

    public ProjectAppService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Project> AddAsync(string name)
    {
        var command = new CreateProjectCommand { Name = name };
        return await _mediator.Send(command);
    }

    public async Task<Project?> GetByIdAsync(string id)
    {
        var query = new GetProjectByIdQuery { Id = id };
        return await _mediator.Send(query);
    }

    public async Task<Project?> GetByNameAsync(string name)
    {
        var query = new GetProjectByNameQuery { Name = name };
        return await _mediator.Send(query);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var query = new GetAllProjectsQuery();
        return await _mediator.Send(query);
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Name = project.Name
        };
        return await _mediator.Send(command);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var command = new DeleteProjectCommand { Id = id };
        return await _mediator.Send(command);
    }

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await GetAllAsync();
    }
    }
}
