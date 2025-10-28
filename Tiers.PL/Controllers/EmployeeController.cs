using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tiers.BLL.ModelVM.Employee;
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

        // 2. Index (GET /Employees)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _employeeService.GetAllAsync();

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes IEnumerable<GetEmployeeVM>
            }

            // You can pass an empty list or return an error view
            return View(new List<GetEmployeeVM>());
        }

        // 3. Details (GET /Employees/Details/5)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            var response = await _employeeService.GetByIdAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes GetEmployeeVM
            }

            return NotFound();
        }

        // 4. Create (GET /Employees/Create)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Call the service to get a VM with the dropdowns populated
            var response = await _employeeService.GetCreateModelAsync();

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes CreateEmployeeVM
            }

            // Handle error (e.g., failed to load departments)
            return RedirectToAction(nameof(Index));
        }

        // 5. Create (POST /Employees/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.CreateAsync(model);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                // If service failed (e.g., file upload error), add error to model
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
            }

            // If we're here, either ModelState is invalid or service failed.
            // We MUST reload the dropdowns before returning the view.
            var dropdownsResponse = await _employeeService.GetCreateModelAsync();
            model.Departments = dropdownsResponse.Result?.Departments ?? new List<SelectListItem>();

            return View(model);
        }

        // 6. Edit (GET /Employees/Edit/5)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();

            // Get the VM populated with employee data AND the dropdown list
            var response = await _employeeService.GetUpdateModelAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes UpdateEmployeeVM
            }

            return NotFound();
        }

        // 7. Edit (POST /Employees/Edit/5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEmployeeVM model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var response = await _employeeService.UpdateAsync(model);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.ErrorMessage);
            }

            // Reload dropdowns if validation fails
            var dropdownsResponse = await _employeeService.GetUpdateModelAsync(model.Id);
            model.Departments = dropdownsResponse.Result?.Departments ?? new List<SelectListItem>();

            return View(model);
        }

        // 8. Delete (GET /Employees/Delete/5)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var response = await _employeeService.GetDeleteModelAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes DeleteEmployeeVM
            }

            return NotFound();
        }

        // 9. Delete (POST /Employees/Delete/5)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteEmployeeVM model)
        {
            var response = await _employeeService.DeleteAsync(model);

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            // If it failed, redisplay the confirmation page with an error
            ModelState.AddModelError(string.Empty, response.ErrorMessage);
            return View(model);
        }

    }
}