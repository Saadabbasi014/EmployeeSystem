using EmployeeSystem.Data;
using EmployeeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filter, string sort)
        {
            List<Employee> employees;

            switch (filter)
            {
                case "Duplicate":
                    employees = GetDuplicateEmployees();
                    break;
                case "Unique":
                    employees = GetUniqueEmployees();
                    break;
                case "All":
                default:
                    employees = await _employeeDbContext.Employees.ToListAsync();
                    break;
            }

            if (sort == "Asc")
            {
                employees = employees.OrderBy(e => e.EmployeeColor).ToList();
            }
            else if (sort == "Desc")
            {
                employees = employees.OrderByDescending(e => e.EmployeeColor).ToList();
            }

            return View(employees);
        }

        private List<Employee> GetDuplicateEmployees()
        {
            var allEmployees = _employeeDbContext.Employees.ToList();

            var duplicateEmployees = allEmployees
                .GroupBy(e => new { e.EmployeeName, e.Department })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            return duplicateEmployees;
        }

        private List<Employee> GetUniqueEmployees()
        {
            var allEmployees = _employeeDbContext.Employees.ToList();

            var uniqueEmployees = allEmployees
                .GroupBy(e => new { e.EmployeeName, e.Department })
                .Where(g => g.Count() == 1)
                .SelectMany(g => g)
                .ToList();

            return uniqueEmployees;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel)
        //{
        //    var employee = new Employee()
        //    {
        //        Id = Guid.NewGuid(),
        //        Department = addEmployeeViewModel.Department,
        //        DOB = addEmployeeViewModel.DOB,
        //        EmployeeColor = addEmployeeViewModel.EmployeeColor,
        //        EmployeeName = addEmployeeViewModel.EmployeeName,
        //        FormNumber = addEmployeeViewModel.FormNumber,
        //        Salary = addEmployeeViewModel.Salary
        //    };

        //   var resp = await _employeeDbContext.Employees.AddAsync(employee);
        //   var res = await _employeeDbContext.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}


        [HttpPost]
        public async Task<JsonResult> AddData([FromBody] AddEmployeeViewModel addEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {

                var employee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Department = addEmployeeViewModel.Department,
                    DOB = addEmployeeViewModel.DOB,
                    EmployeeColor = addEmployeeViewModel.EmployeeColor,
                    EmployeeName = addEmployeeViewModel.EmployeeName,
                    FormNumber = addEmployeeViewModel.FormNumber,
                    Salary = addEmployeeViewModel.Salary
                };

                await _employeeDbContext.Employees.AddAsync(employee);
                var response = await _employeeDbContext.SaveChangesAsync();


                if (response > 0)
                {
                    return Json(new
                    {
                        status = true,
                        message = "Employee added successfully."
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "Error occurred while adding employee."
                    });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return (JsonResult)errors;
            }

        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await _employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModal = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Department = employee.Department,
                    DOB = employee.DOB,
                    EmployeeColor = employee.EmployeeColor,
                    EmployeeName = employee.EmployeeName,
                    FormNumber = employee.FormNumber,
                    Salary = employee.Salary
                };
                return await Task.Run(() => View("View", viewModal));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Department = model.Department;
                employee.DOB = model.DOB;
                employee.EmployeeColor = model.EmployeeColor;
                employee.EmployeeName = model.EmployeeName;
                employee.FormNumber = model.FormNumber;
                employee.Salary = model.Salary;

                await _employeeDbContext.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                _employeeDbContext.Employees.Remove(employee);
                await _employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
