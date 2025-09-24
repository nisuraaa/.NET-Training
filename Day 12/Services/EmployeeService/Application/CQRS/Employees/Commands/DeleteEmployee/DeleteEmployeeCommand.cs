using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
