using System.Linq.Expressions;
using Tiers.DAL.Repo.Abstraction;

namespace Tiers.DAL.Repo.Implementation
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly ApplicationDbContext _db;

        public DepartmentRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync(Expression<Func<Department, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return await _db.Departments.Where(filter).ToListAsync();
                }
                return await _db.Departments.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            try
            {
                var dep = await _db.Departments.FindAsync(id);
                if (dep != null)
                {
                    return dep;
                }
                throw new KeyNotFoundException($"Department with Id {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddAsync(Department newDepartment)
        {
            try
            {
                if (newDepartment == null)
                {
                    return false;
                }
                var result = await _db.Departments.AddAsync(newDepartment);
                if (result.Entity.Id <= 0) // care about this, id is given only after save changes
                {
                    return false;
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Department newDepartment)
        {
            try
            {
                if (newDepartment == null)
                {
                    return false;
                }
                var oldDepartment = await _db.Departments.FindAsync(newDepartment.Id);
                if (oldDepartment == null)
                {
                    return false;
                }
                bool result = oldDepartment.Update(newDepartment.Name, newDepartment.Area, "Fady");
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
                var dep = await _db.Departments.FindAsync(id);
                if (dep == null)
                {
                    return false;
                }
                bool result = dep.ToggleDelete(dep.DeletedBy ?? "Fady");
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
