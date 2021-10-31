using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    /// <summary>
    /// Create a new type, ReportingStructure, that has two properties: employee and numberOfReports.
    //    For the field "numberOfReports", this should equal the total number of reports under a given employee.The number of
    //    reports is determined to be the number of directReports for an employee and all of their direct reports. For example,
    //    given the following employee structure:
    //```
    //                        John Lennon
    //                /               \
    //             Paul McCartney         Ringo Starr
    //                               /        \
    //                              Pete Best     George Harrison
    //```
    //The numberOfReports for employee John Lennon (employeeId: 16a596ae-edd3-4847-99fe-c4518e82c86f) would be equal to 4. 
    //This new type should have a new REST endpoint created for it.This new endpoint should accept an employeeId and return 
    //the fully filled out ReportingStructure for the specified employeeId.The values should be computed on the fly and will
    //not be persisted.
    /// </summary>
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }

    
    }
}
