using System;
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

        public IEnumerable<Employee> GetAll(Expression<Func<Employee, bool>>? Filter = null)
        {
            try
            {
                if (Filter != null)
                {
                    return _db.Employees.Where(Filter).ToList();
                }
                return _db.Employees.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Employee GetById(int id)
        {
            try
            {
                var emp = _db.Employees.Find(id);
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
        public bool Add(Employee newEmployee)
        {
            try
            {
                if (newEmployee == null)
                {
                    return false;
                }
                var result = _db.Employees.Add(newEmployee);
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
        public bool Update(Employee newEmployee)
        {
            try
            {
                if (newEmployee == null)
                {
                    return false;
                }
                var oldEmployee = _db.Employees.Find(newEmployee.Id);
                if (oldEmployee == null)
                {
                    return false;
                }
                bool result = oldEmployee.Update(newEmployee.Name, newEmployee.Salary, newEmployee.DepartmentId, "Fady");
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
                var emp = _db.Employees.Find(id);
                if (emp == null)
                {
                    return false;
                }
                bool result = emp.ToggleDelete(emp.DeletedBy ?? "Fady");
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
