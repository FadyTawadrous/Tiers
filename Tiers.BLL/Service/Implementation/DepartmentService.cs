using Tiers.BLL.ModelVM.Department;
using Tiers.BLL.Service.Abstraction;
using Tiers.DAL.Entity;
using Tiers.DAL.Repo.Abstraction;

namespace Tiers.BLL.Service.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepo departmentRepo, IMapper mapper)
        {
            _departmentRepo = departmentRepo;
            _mapper = mapper;
        }

        #region Read Methods

        public async Task<ResponseResult<IEnumerable<GetDepartmentVM>>> GetAllAsync()
        {
            try
            {
                // Include Employees so AutoMapper can get the EmployeeCount
                var departments = await _departmentRepo.GetAllAsync(d => !d.IsDeleted, includes: d => d.Employees);
                var mappedResult = _mapper.Map<IEnumerable<GetDepartmentVM>>(departments);
                return new ResponseResult<IEnumerable<GetDepartmentVM>>(mappedResult, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetDepartmentVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetDepartmentVM>> GetByIdAsync(int id)
        {
            try
            {
                var department = await _departmentRepo.GetByIdAsync(id);
                if (department == null || department.IsDeleted)
                {
                    return new ResponseResult<GetDepartmentVM>(null, "Department not found.", false);
                }

                var mapped = _mapper.Map<GetDepartmentVM>(department);
                return new ResponseResult<GetDepartmentVM>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetDepartmentVM>(null, ex.Message, false);
            }
        }

        #endregion

        #region Create Methods

        public async Task<ResponseResult<bool>> CreateAsync(CreateDepartmentVM model)
        {
            try
            {
                // 1. Manually create entity using constructor (NO AutoMapper)
                var department = new Department(
                    model.Name,
                    model.Area,
                    model.CreatedBy
                );

                // 2. Call repo to Add
                var result = await _departmentRepo.AddAsync(department);

                // 3. Return response
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }
                else
                {
                    return new ResponseResult<bool>(false, "Failed to save department.", false);
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        #endregion

        #region Update Methods

        public async Task<ResponseResult<UpdateDepartmentVM>> GetUpdateModelAsync(int id)
        {
            try
            {
                // 1. Get the department entity
                var department = await _departmentRepo.GetByIdAsync(id);
                if (department == null || department.IsDeleted)
                {
                    return new ResponseResult<UpdateDepartmentVM>(null, "Department not found.", false);
                }

                // 2. Map Entity -> VM
                var model = _mapper.Map<UpdateDepartmentVM>(department);
                return new ResponseResult<UpdateDepartmentVM>(model, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<UpdateDepartmentVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> UpdateAsync(UpdateDepartmentVM model)
        {
            try
            {
                // 1. Get the tracked entity
                var deptToUpdate = await _departmentRepo.GetByIdAsync(model.Id);
                if (deptToUpdate == null)
                {
                    return new ResponseResult<bool>(false, "Department not found.", false);
                }

                // 2. Call the entity's OWN update method
                deptToUpdate.Update(model.Name, model.Area, model.UpdatedBy);

                // 3. Save changes
                var saveResult = await _departmentRepo.SaveChangesAsync();
                return new ResponseResult<bool>(saveResult, null, saveResult);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        #endregion

        #region Delete Methods

        public async Task<ResponseResult<DeleteDepartmentVM>> GetDeleteModelAsync(int id)
        {
            try
            {
                // 1.Get the employee entity
                var department = await _departmentRepo.GetByIdAsync(id);
                if (department == null || department.IsDeleted)
                {
                    return new ResponseResult<DeleteDepartmentVM>(null, "Department not found.", false);
                }

                // 2. Map Entity -> VM
                var model = _mapper.Map<DeleteDepartmentVM>(department);
                return new ResponseResult<DeleteDepartmentVM>(model, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<DeleteDepartmentVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteAsync(DeleteDepartmentVM model)
        {
            try
            {
                // 1. Get the tracked entity
                var deptToDelete = await _departmentRepo.GetByIdAsync(model.Id);
                if (deptToDelete == null)
                {
                    return new ResponseResult<bool>(false, "Department not found.", false);
                }

                // 2. Call the entity's logic
                bool toggleResult = deptToDelete.ToggleDelete(model.DeletedBy);
                if (!toggleResult)
                {
                    return new ResponseResult<bool>(false, "Failed to toggle delete status.", false);
                }

                // 3. Save changes
                var saveResult = await _departmentRepo.SaveChangesAsync();
                return new ResponseResult<bool>(saveResult, null, saveResult);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        #endregion
    }
}
