using Tiers.BLL.Helper;
using Tiers.BLL.ModelVM.Employee;
using Tiers.BLL.Service.Abstraction;
using Tiers.DAL.Entity;
using Tiers.DAL.Repo.Abstraction;

namespace Tiers.BLL.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepo employeeRepo, IDepartmentRepo departmentRepo, IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _departmentRepo = departmentRepo;
            _mapper = mapper;
        }
        public async Task<ResponseResult<IEnumerable<GetEmployeeVM>>> GetAllAsync()
        {
            try
            {
                var result = await _employeeRepo.GetAllAsync(e => !e.IsDeleted, includes: e => e.Department);
                var mappedResult = _mapper.Map<IEnumerable<GetEmployeeVM>>(result);

                return new ResponseResult<IEnumerable<GetEmployeeVM>>(mappedResult, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetEmployeeVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetEmployeeVM>> GetByIdAsync(int id)
        {
            try
            {
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null || employee.IsDeleted)
                {
                    return new ResponseResult<GetEmployeeVM>(null, "Employee not found.", false);
                }

                var mapped = _mapper.Map<GetEmployeeVM>(employee);
                return new ResponseResult<GetEmployeeVM>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetEmployeeVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<CreateEmployeeVM>> GetCreateModelAsync()
        {
            try
            {
                var departments = await _departmentRepo.GetAllAsync(d => !d.IsDeleted);
                var departmentSelectList = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

                var model = new CreateEmployeeVM
                {
                    Departments = departmentSelectList
                };

                return new ResponseResult<CreateEmployeeVM>(model, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<CreateEmployeeVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> CreateAsync(CreateEmployeeVM newEmployee)
        {
            try
            {
                //1- Handle file upload
                string? imageUrl = null;
                if (newEmployee.Image != null)
                {
                    try
                    {
                        imageUrl = Upload.UploadFile("Images", newEmployee.Image);
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<bool>(false, $"File upload failed: {ex.Message}", false);
                    }
                }

                //2- Create new Employee using constructor
                Employee employee = new Employee(
                    newEmployee.Name,
                    newEmployee.Salary,
                    newEmployee.Age,
                    imageUrl,
                    newEmployee.DepartmentId,
                    newEmployee.CreatedBy
                    );

                //3- Call the repo
                var result = await _employeeRepo.AddAsync(employee);

                //4- Return the response
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }
                else
                {
                    return new ResponseResult<bool>(false, "Failed to save employee to the database.", false);
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<UpdateEmployeeVM>> GetUpdateModelAsync(int id)
        {
            try
            {
                // 1. Get the employee entity
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null || employee.IsDeleted)
                {
                    return new ResponseResult<UpdateEmployeeVM>(null, "Employee not found.", false);
                }

                // 2. Map Entity -> VM
                var model = _mapper.Map<UpdateEmployeeVM>(employee);

                // 3. Get departments for the dropdown
                var departments = await _departmentRepo.GetAllAsync(d => !d.IsDeleted);
                model.Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

                return new ResponseResult<UpdateEmployeeVM>(model, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<UpdateEmployeeVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> UpdateAsync(UpdateEmployeeVM model)
        {
            try
            {
                // 1. Get the tracked entity from the repo
                var employeeToUpdate = await _employeeRepo.GetByIdAsync(model.Id);
                if (employeeToUpdate == null)
                {
                    return new ResponseResult<bool>(false, "Employee not found.", false);
                }

                // 2. Handle file logic
                string? newImageUrl = employeeToUpdate.ImageUrl;
                if (model.Image != null)
                {
                    try
                    {
                        newImageUrl = Upload.UploadFile("Images", model.Image);

                        if (!string.IsNullOrEmpty(employeeToUpdate.ImageUrl))
                        {
                            Upload.RemoveFile("Images", employeeToUpdate.ImageUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<bool>(false, $"File update failed: {ex.Message}", false);
                    }
                }

                // 3. Call the entity's OWN methods to update its state
                employeeToUpdate.Update(model.Name, model.Salary, model.DepartmentId, model.UpdatedBy, model.Age, newImageUrl);

                // 4. Save changes
                var result = await _employeeRepo.SaveChangesAsync(); // Use the new repo method

                return new ResponseResult<bool>(result, null, result);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }


        public async Task<ResponseResult<DeleteEmployeeVM>> GetDeleteModelAsync(int id)
        {
            try
            {
                // 1. Get the employee entity
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null || employee.IsDeleted)
                {
                    return new ResponseResult<DeleteEmployeeVM>(null, "Employee not found.", false);
                }

                // 2. Map Entity -> VM
                var model = _mapper.Map<DeleteEmployeeVM>(employee);

                return new ResponseResult<DeleteEmployeeVM>(model, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<DeleteEmployeeVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteAsync(DeleteEmployeeVM model)
        {
            try
            {
                // 1. Get the tracked entity
                var employeeToDelete = await _employeeRepo.GetByIdAsync(model.Id);
                if (employeeToDelete == null)
                {
                    return new ResponseResult<bool>(false, "Employee not found.", false);
                }

                // 2. Call the entity's logic
                bool toggleResult = employeeToDelete.ToggleDelete(model.DeletedBy);
                if (!toggleResult)
                {
                    return new ResponseResult<bool>(false, "Failed to toggle delete status.", false);
                }

                // 3. Save changes
                var result = await _employeeRepo.SaveChangesAsync(); // Use the new repo method

                // 4. Delete the file from disk (only if delete was successful)
                if (result && !string.IsNullOrEmpty(employeeToDelete.ImageUrl))
                {
                    Upload.RemoveFile("Images", employeeToDelete.ImageUrl);
                }

                return new ResponseResult<bool>(result, null, result);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }


    }
}
