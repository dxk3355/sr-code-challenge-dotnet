using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);

        /// <summary>
        /// Get the reporting structure for an employee
        /// </summary>
        /// <param name="employee">Employee to get structure for</param>
        /// <returns>Filled out reporting structure</returns>
        ReportingStructure GetReportingStructure(Employee employee);
    }
}
