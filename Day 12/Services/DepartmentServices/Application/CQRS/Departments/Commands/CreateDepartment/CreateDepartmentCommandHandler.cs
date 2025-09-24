using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Department>
{
    private readonly IDepartmentRepository _departmentRepository;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<Department> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department();
        department.Initialize(Guid.NewGuid().ToString(), request.Name);
        
        return await _departmentRepository.AddAsync(department);
    }
}
