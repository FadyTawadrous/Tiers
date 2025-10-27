using System.Linq.Expressions;

namespace Tiers.DAL.Repo.Abstraction
{
    public interface IDepartmentRepo
    {
        // Query Methods
        Task<Department> GetById(int id);
        Task<IEnumerable<Department>> GetAll(Expression<Func<Department, bool>>? Filter = null);

        // Command Methods
        Task<bool> Add(Department newDepartment);
        Task<bool> Update(Department newDepartment);
        Task<bool> ToggleDeleteStatus(int id);
    }
}
