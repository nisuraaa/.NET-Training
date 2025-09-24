using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeeSummaries;

public class GetEmployeeSummariesQuery : IRequest<List<EmployeeSummary>>
{
}
