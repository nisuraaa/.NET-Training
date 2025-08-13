using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagment.Tests
{
    public class ManagerTests
    {
         public void Manager_DisplayDetails_WritesToConsole()
        {
            var manager = new Manager
            {
                Id = "M001",
                Name = "Jane Smith",
                Age = 35,
                Department = "HR",
                TeamSize = 10
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            manager.DisplayDetails();
            var output = stringWriter.ToString();

            Assert.Contains("[Manager]", output);
            Assert.Contains("ID=M001", output);
            Assert.Contains("Name=Jane Smith", output);
            Assert.Contains("Age=35", output);
            Assert.Contains("Dept=HR", output);
            Assert.Contains("TeamSize=10", output);
        }
    }
}
