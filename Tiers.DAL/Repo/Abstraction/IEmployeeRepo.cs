using System.Linq.Expressions;

namespace Tiers.DAL.Repo.Abstraction
{
    public interface IEmployeeRepo
    {
        // Query Methods
        Employee GetById(int id);
        IEnumerable<Employee> GetAll(Expression<Func<Employee, bool>>? Filter = null);

        // Command Methods
        bool Add(Employee newEmployee);
        bool Update(Employee newEmployee);
        bool ToggleDeleteStatus(int id);

    }
}
