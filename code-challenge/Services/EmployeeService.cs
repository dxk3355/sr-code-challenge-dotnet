using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            _logger.LogInformation(nameof(EmployeeService) + "GetById" + id);
            if (!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        /// <summary>
        /// Get the reporting structure for an employee
        /// </summary>
        /// <param name="employee">Employee to get structure for</param>
        /// <returns>Filled out reporting structure</returns>
        public ReportingStructure GetReportingStructure(Employee employee)
        {
            return new ReportingStructure() { Employee = employee, NumberOfReports = GetNumberOfReports(employee) };
        }

        /// <summary>
        /// Recursively get the counts of direct reports
        /// </summary>
        /// <param name="employee">Employee that has reports</param>
        /// <returns>0 if null, otherwise the total number of direct reports as an integer</returns>
        private int GetNumberOfReports(Employee employee)
        {
            if (employee == null || employee.DirectReports == null) { return 0; }
            int total = employee.DirectReports.Count;
            foreach (Employee report in employee.DirectReports)
            {
                total += GetNumberOfReports(report);
            }
            return total;
        }

    }
}
