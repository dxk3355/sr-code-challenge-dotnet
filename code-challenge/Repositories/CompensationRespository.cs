using challenge.Data;
using challenge.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public CompensationRepository(ILogger<IEmployeeRepository> logger, CompensationContext compContext)
        {
            _compensationContext = compContext;
            _logger = logger;
        }

        /// <summary>
        /// Create a new compensation
        /// </summary>
        /// <param name="compensation">Compensation to add</param>
        /// <returns>Compensation added but not saved</returns>
        public Compensation Add(Compensation compensation)
        {
            compensation.Id = Guid.NewGuid().ToString();
            _logger.LogInformation("CompensationRepository:Add:id" + compensation.Id + "\t" + compensation.EmployeeId);
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the compensation by the ID
        /// </summary>
        /// <param name="employee_id">GUID id for compensation record</param>
        /// <returns>Compensation object that matches the employees</returns>
        public Compensation GetCompensationById(string id)
        {
            _logger.LogInformation("CompensationRepository:GetCompensationById:id=" + id);
            return _compensationContext.Compensations.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Gets the most recent compensation in order of effective date for the specified employee
        /// </summary>
        /// <param name="employee_id">employee id for compensation record</param>
        /// <returns>Compensation object that is of the most recent compensation for that employee id</returns>
        public Compensation GetCompensationByEmployeeId(string employee_id)
        {
            _logger.LogInformation("CompensationRepository:GetCompensationByEmployeeId:employee_id=" + employee_id);
            return _compensationContext.Compensations.OrderByDescending(c => c.EffectiveDate).FirstOrDefault(c => c.EmployeeId == employee_id);
        }
    }
}
