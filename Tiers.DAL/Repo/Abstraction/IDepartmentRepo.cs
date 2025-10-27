using System.Linq.Expressions;

namespace Tiers.DAL.Repo.Abstraction
{
    public interface IDepartmentRepo
    {
        // Query Methods
        Department GetById(int id);
        IEnumerable<Department> GetAll(Expression<Func<Department, bool>>? Filter = null);

        // Command Methods
        bool Add(Department newDepartment);
        bool Update(Department newDepartment);
        bool ToggleDeleteStatus(int id);
    }
}
