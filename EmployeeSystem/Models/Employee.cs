using System;

namespace EmployeeSystem.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Department { get; set; }
        public DateTime DOB { get; set; }
        public string EmployeeColor { get; set; }
        public string EmployeeName { get; set; }
        public int FormNumber { get; set; }
        public long Salary { get; set; }

        public List<long> ProjectedSalaries()
        {
            var projectedSalaries = new List<long>();

            DateTime startingMonth = new DateTime(2021, 7, 1);
            DateTime currentMonth = startingMonth;
            long previousSalary = this.Salary;

            for (int i = 0; i < 6; i++)
            {
                long increasedSalary = (long)(previousSalary * 1.05);
                projectedSalaries.Add(increasedSalary);

                previousSalary = increasedSalary;
                currentMonth = currentMonth.AddMonths(1);
            }

            return projectedSalaries;
        }
    }
}