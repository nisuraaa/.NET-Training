using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Queries.GetDepartmentByName;

public class GetDepartmentByNameQueryHandler : IRequestHandler<GetDepartmentByNameQuery, Department?>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentByNameQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<Department?> Handle(GetDepartmentByNameQuery request, CancellationToken cancellationToken)
    {
        return await _departmentRepository.GetByNameAsync(request.Name);
    }
}
