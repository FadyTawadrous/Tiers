using Microsoft.AspNetCore.Mvc;
using Tiers.BLL.Service.Abstraction;

namespace Tiers.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            var ActiveEmployees = _employeeService.GetActiveEmployees();
            return View(ActiveEmployees);
        }

        public IActionResult Details() {
            var employee = _employeeService.get(1);
            return View(employee);
        }
}
