using System;
using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    public class Compensation
    {
        [Key]
        public string Id { get; set; }  // Need id field for EF, alternately we would use the employee ID as PK from the employee but since the effective date is here I don't think that applies as they may have more than one.
        [Required]
        public String EmployeeId { get; set; }
        public decimal Salary { get; set; } // Using Decimal as they may want to put an hourly rate in instead a yearly salary, or you want to pay a salary in cents
        public DateTime EffectiveDate { get; set; }
    }
}
