using System.Linq.Expressions;

namespace Tiers.DAL.Repo.Abstraction
{
    public interface IDepartmentRepo
    {
        // Query Methods
        Task<Department?> GetByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync(Expression<Func<Department, bool>>? Filter = null);

        // Command Methods
        Task<bool> AddAsync(Department newDepartment);
        Task<bool> UpdateAsync(Department newDepartment);
        Task<bool> ToggleDeleteStatusAsync(int id);
    }
}
