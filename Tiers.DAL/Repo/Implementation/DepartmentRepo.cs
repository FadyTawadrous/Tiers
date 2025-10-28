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

        public async Task<IEnumerable<Department>> GetAllAsync(Expression<Func<Department, bool>>? filter = null,
            params Expression<Func<Department, object>>[] includes)
        {
            try
            {
                IQueryable<Department> query = _db.Departments;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.ToListAsync();
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
                var dep = await _db.Departments.Include(e => e.Employees).FirstOrDefaultAsync(e => e.Id == id);
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
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;
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
                bool result = oldDepartment.Update(newDepartment.Name, newDepartment.Area, newDepartment.UpdatedBy);
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

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                // SaveChangesAsync returns the number of rows affected
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
