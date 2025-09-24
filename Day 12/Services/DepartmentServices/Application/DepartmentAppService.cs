using MediatR;
using DepartmentServices.Application.CQRS.Departments.Commands.CreateDepartment;
using DepartmentServices.Application.CQRS.Departments.Commands.UpdateDepartment;
using DepartmentServices.Application.CQRS.Departments.Commands.DeleteDepartment;
using DepartmentServices.Application.CQRS.Departments.Queries.GetDepartmentById;
using DepartmentServices.Application.CQRS.Departments.Queries.GetDepartmentByName;
using DepartmentServices.Application.CQRS.Departments.Queries.GetAllDepartments;

namespace DepartmentServices.Application;

public class DepartmentAppService : IDepartmentAppService
{
    private readonly IMediator _mediator;

    public DepartmentAppService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Department> AddAsync(string name)
    {
        var command = new CreateDepartmentCommand { Name = name };
        return await _mediator.Send(command);
    }

    public async Task<Department?> GetByIdAsync(string id)
    {
        var query = new GetDepartmentByIdQuery { Id = id };
        return await _mediator.Send(query);
    }

    public async Task<Department?> GetByNameAsync(string name)
    {
        var query = new GetDepartmentByNameQuery { Name = name };
        return await _mediator.Send(query);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        var query = new GetAllDepartmentsQuery();
        return await _mediator.Send(query);
    }

    public async Task<Department> UpdateAsync(Department department)
    {
        var command = new UpdateDepartmentCommand
        {
            Id = department.Id,
            Name = department.Name
        };
        return await _mediator.Send(command);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var command = new DeleteDepartmentCommand { Id = id };
        return await _mediator.Send(command);
    }
}
