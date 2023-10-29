using System;
using System.ComponentModel.DataAnnotations;
namespace EmployeeSystem.Models

{
    public class AddEmployeeViewModel
    {
        [Required]
        public string EmployeeName { get; set; }
        public string EmployeeColor { get; set; }
        public DateTime DOB { get; set; }
        [Required]
        public string Department { get; set; }
        public int FormNumber { get; set; }
        [Required]
        public long Salary { get; set; }
    }
}
