using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTest
{
    internal class EmployeeServiceTest
    {
        public void TestEmployeeCreation()
        {
            var employee = new Employee { Id="1234", Name="Nisura", Age= 30, Department="IT"};
        }
    }
}
