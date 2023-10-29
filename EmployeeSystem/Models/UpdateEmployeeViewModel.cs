namespace EmployeeSystem.Models
{
    public class UpdateEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Department { get; set; }
        public DateTime DOB { get; set; }
        public string EmployeeColor { get; set; }
        public string EmployeeName { get; set; }
        public int FormNumber { get; set; }
        public long Salary { get; set; }
    }
}
