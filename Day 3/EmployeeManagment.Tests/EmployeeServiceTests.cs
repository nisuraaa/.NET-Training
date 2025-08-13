using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagment.Tests
{
    public class EmployeeServiceTests
    {
        [Fact]

        public void AddEmployee_NewEmployee_StoresEmployee()
        {
            var service = new EmployeeService();
            var employee = new Employee { Id = "E001", Name = "John Doe", Age = 30, Department = "IT" };

            service.AddEmployee(employee);
            var result = service.GetEmployeeById("E001");

            Assert.NotNull(result);
            Assert.Equal("E001", result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal(30, result.Age);
            Assert.Equal("IT", result.Department);
        }

        [Fact]
        public void GetEmployeeById_ExistingId_ReturnsEmployee()
        {
            var service = new EmployeeService();
            var employee = new Employee { Id = "E001", Name = "John Doe", Age = 30, Department = "IT" };
            service.AddEmployee(employee);

            var result = service.GetEmployeeById("E001");

            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }
    }
}
