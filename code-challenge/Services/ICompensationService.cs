using challenge.Models;
using System;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation Create(Compensation compensation);

        Compensation GetById(string id);

        /// <summary>
        /// Gets the most recent compensation in order of effective date for the specified employee
        /// </summary>
        /// <param name="employee_id">employee id for compensation record</param>
        /// <returns>Compensation object that is of the most recent compensation for that employee id</returns>
        Compensation GetByEmployeeId(string employee_id);
    }
}
