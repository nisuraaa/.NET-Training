using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Xunit;

namespace EmployeeManagment.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void Employee_DisplayDetails_WritesToConsole()
        {
            var employee = new Employee
            {
                Id = "E001",
                Name = "John Doe",
                Age = 30,
                Department = "IT"
            };

            var originalOutput = Console.Out;
            using var stringWriter = new StringWriter();
            try
            {
                Console.SetOut(stringWriter);
                employee.DisplayDetails();
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

            var output = stringWriter.ToString();

            // Assert
            Assert.Contains("[Employee]", output);
            Assert.Contains("ID=E001", output);
            Assert.Contains("Name=John Doe", output);
            Assert.Contains("Age=30", output);
            Assert.Contains("Dept=IT", output);
        }

        [Fact]
        public void SerializeEmployees_PrintsCorrectJson()
        {
            var mockEmployeeService = new Mock<IEmployee>();
            var employeeList = new List<Employee>
            {
                new Employee { Id = "E001", Name = "John Doe", Age = 30, Department = "IT" },
                new Manager { Id = "M001", Name = "Jane Smith", Age = 35, Department = "HR", TeamSize = 5 }
            };

            mockEmployeeService.Setup(m => m.GetAllEmployees()).Returns(employeeList);

            var originalOutput = Console.Out;
            using var consoleOutput = new StringWriter();
            try
            {
                Console.SetOut(consoleOutput);

                var methodInfo = typeof(Program).GetMethod("SerializeEmployees", BindingFlags.NonPublic | BindingFlags.Static);
                if (methodInfo == null)
                {
                    Assert.Fail("SerializeEmployees method not found in Program class");
                }

                methodInfo.Invoke(null, new object[] { mockEmployeeService.Object });
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

            string output = consoleOutput.ToString();
            Assert.Contains("\"Id\": \"E001\"", output);
            Assert.Contains("\"Name\": \"John Doe\"", output);
            Assert.Contains("\"Id\": \"M001\"", output);
            Assert.Contains("\"TeamSize\": 5", output);

            mockEmployeeService.Verify(m => m.GetAllEmployees(), Times.Once);
        }
    }
}
