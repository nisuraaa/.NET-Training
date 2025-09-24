using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<Department>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<Department>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        return await _departmentRepository.GetAllAsync();
    }
}
