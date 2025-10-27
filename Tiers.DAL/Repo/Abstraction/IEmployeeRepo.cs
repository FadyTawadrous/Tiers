using System.Linq.Expressions;

namespace Tiers.DAL.Repo.Abstraction
{
    public interface IEmployeeRepo
    {
        // Query Methods
        Task<Employee> GetById(int id);
        Task<IEnumerable<Employee>> GetAll(Expression<Func<Employee, bool>>? Filter = null);

        // Command Methods
        Task<bool> Add(Employee newEmployee);
        Task<bool> Update(Employee newEmployee);
        Task<bool> ToggleDeleteStatus(int id);

    }
}
