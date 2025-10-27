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

        public IEnumerable<Department> GetAll(Expression<Func<Department, bool>>? Filter = null)
        {
            try
            {
                if (Filter != null)
                {
                    return _db.Departments.Where(Filter).ToList();
                }
                return _db.Departments.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Department GetById(int id)
        {
            try
            {
                var dep = _db.Departments.Find(id);
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
        public bool Add(Department newDepartment)
        {
            try
            {
                if (newDepartment == null)
                {
                    return false;
                }
                var result = _db.Departments.Add(newDepartment);
                if (result.Entity.Id <= 0)
                {
                    return false;
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Update(Department newDepartment)
        {
            try
            {
                if (newDepartment == null)
                {
                    return false;
                }
                var oldDepartment = _db.Departments.Find(newDepartment.Id);
                if (oldDepartment == null)
                {
                    return false;
                }
                bool result = oldDepartment.Update(newDepartment.Name, newDepartment.Area, "Fady");
                if (result)
                {
                    _db.SaveChanges();
                    return result;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ToggleDeleteStatus(int id)
        {
            try
            {
                var dep = _db.Departments.Find(id);
                if (dep == null)
                {
                    return false;
                }
                bool result = dep.ToggleDelete(dep.DeletedBy ?? "Fady");
                if (result)
                {
                    _db.SaveChanges();
                    return result;
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
