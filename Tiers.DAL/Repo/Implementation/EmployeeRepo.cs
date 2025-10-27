using System.Linq.Expressions;
using Tiers.DAL.Repo.Abstraction;

namespace Tiers.DAL.Repo.Implementation
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(Expression<Func<Employee, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return await _db.Employees.Where(filter).ToListAsync();
                }
                return await _db.Employees.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            try
            {
                var emp = await _db.Employees.FindAsync(id);
                if (emp != null)
                {
                    return emp;
                }
                throw new KeyNotFoundException($"Employee with Id {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddAsync(Employee newEmployee)
        {
            try
            {
                if (newEmployee == null)
                {
                    return false;
                }
                var result = await _db.Employees.AddAsync(newEmployee);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Employee newEmployee)
        {
            try
            {
                if (newEmployee == null)
                {
                    return false;
                }
                var oldEmployee = await _db.Employees.FindAsync(newEmployee.Id);
                if (oldEmployee == null)
                {
                    return false;
                }
                bool result = oldEmployee.Update(newEmployee.Name, newEmployee.Salary, newEmployee.DepartmentId, "Fady");
                if (result)
                {
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ToggleDeleteStatusAsync(int id)
        {
            try
            {
                var emp = await _db.Employees.FindAsync(id);
                if (emp == null)
                {
                    return false;
                }
                bool result = emp.ToggleDelete(emp.DeletedBy ?? "Fady");
                if (result)
                {
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
