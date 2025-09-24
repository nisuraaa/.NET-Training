using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeesByAgeRange;

public class GetEmployeesByAgeRangeQuery : IRequest<List<Employee>>
{
    public int LowerAgeLimit { get; set; }
    public int UpperAgeLimit { get; set; }
}
