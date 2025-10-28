using Microsoft.AspNetCore.Mvc;
using Tiers.BLL.ModelVM.Department;
using Tiers.BLL.Service.Abstraction;

namespace Tiers.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        // 1. Inject the BLL service
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // 2. Index (GET /Departments)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _departmentService.GetAllAsync();

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes IEnumerable<GetDepartmentVM>
            }

            // Return an empty list on failure
            return View(new List<GetDepartmentVM>());
        }

        // 3. Details (GET /Departments/Details/5)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            var response = await _departmentService.GetByIdAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes GetDepartmentVM
            }

            return NotFound();
        }

        // 4. Create (GET /Departments/Create)
        [HttpGet]
        public IActionResult Create()
        {
            // No service call needed, just return a new empty VM
            return View(new CreateDepartmentVM());
        }

        // 5. Create (POST /Departments/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _departmentService.CreateAsync(model);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                // If service failed, add error to model
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
            }

            // If we're here, ModelState is invalid or service failed.
            // No need to reload dropdowns, so just return the view.
            return View(model);
        }

        // 6. Edit (GET /Departments/Edit/5)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();

            // Get the VM populated with department data
            var response = await _departmentService.GetUpdateModelAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes UpdateDepartmentVM
            }

            return NotFound();
        }

        // 7. Edit (POST /Departments/Edit/5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDepartmentVM model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var response = await _departmentService.UpdateAsync(model);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, response.ErrorMessage);
            }

            // No dropdowns to reload, just return the view
            return View(model);
        }

        // 8. Delete (GET /Departments/Delete/5)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var response = await _departmentService.GetDeleteModelAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result); // Passes DeleteDepartmentVM
            }

            return NotFound();
        }

        // 9. Delete (POST /Departments/Delete/5)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteDepartmentVM model)
        {
            var response = await _departmentService.DeleteAsync(model);

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
