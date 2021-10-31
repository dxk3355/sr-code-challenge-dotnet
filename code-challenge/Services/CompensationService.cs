using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Services
{
    public class CompensationService: ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if (compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        /// <summary>
        /// Returns the compensation object or null if not found or an empty string is provided
        /// </summary>
        /// <param name="id">Guid Id of the compensation</param>
        /// <returns>Compensation Object that matches the ID</returns>
        Compensation ICompensationService.GetById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
              return  _compensationRepository.GetCompensationById(id);
            }

            return null;
        }

             /// <summary>
        /// Gets the most recent compensation in order of effective date for the specified employee
        /// </summary>
        /// <param name="employee_id">employee id for compensation record</param>
        /// <returns>Compensation object that is of the most recent compensation for that employee id</returns>
        Compensation ICompensationService.GetByEmployeeId(string employee_id)
        {
            if (!String.IsNullOrEmpty(employee_id))
            {
                return _compensationRepository.GetCompensationByEmployeeId(employee_id);
            }

            return null;
        }

    }
}
