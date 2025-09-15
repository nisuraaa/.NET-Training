using System.ComponentModel.DataAnnotations;

public class EmployeeSummary
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Salary { get; set; } = string.Empty;
}

public class EmployeeBasicInfo
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class EmployeeContactInfo
{
    public string FullName { get; set; } = string.Empty;
    public string DepartmentInfo { get; set; } = string.Empty;
}

public class CreateEmployeeRequest
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public decimal Salary { get; set; }

    public string isManager { get; set; } = "false";

    public int? teamCount { get; set; } = 0;

    public string? DepartmentName { get; set; }

    public string? ProjectName { get; set; }
}

public class UpdateEmployeeRequest
{

    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public decimal Salary { get; set; }

    public string? DepartmentName { get; set; }
}

public class EmployeeDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string DepartmentName { get; set; }
}