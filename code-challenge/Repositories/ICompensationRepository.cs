using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation);

        /// <summary>
        /// Gets the compensation by the ID
        /// </summary>
        /// <param name="employee_id">GUID id for compensation record</param>
        /// <returns>Compensation object that matches the employees</returns>
        Compensation GetCompensationById(string id);

        /// <summary>
        /// Gets the most recent compensation in order of effective date for the specified employee
        /// </summary>
        /// <param name="employee_id">employee id for compensation record</param>
        /// <returns>Compensation object that is of the most recent compensation for that employee id</returns>
        Compensation GetCompensationByEmployeeId(string employee_id);
        Task SaveAsync();
    }
}
