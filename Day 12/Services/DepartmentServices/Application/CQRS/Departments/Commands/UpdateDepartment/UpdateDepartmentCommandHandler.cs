using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Department>
{
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<Department> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var existingDepartment = await _departmentRepository.GetByIdAsync(request.Id);
        if (existingDepartment == null)
        {
            throw new KeyNotFoundException($"Department with ID {request.Id} not found");
        }

        existingDepartment.SetName(request.Name);
        return await _departmentRepository.UpdateAsync(existingDepartment);
    }
}
