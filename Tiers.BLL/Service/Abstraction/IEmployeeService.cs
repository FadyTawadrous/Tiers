using Tiers.BLL.ModelVM.Employee;
using Tiers.BLL.Responses;

namespace Tiers.BLL.Service.Abstraction
{
    public interface IEmployeeService
    {
        ResponseResult<List<GetEmployeeVM>> GetActiveEmployees();
        ResponseResult<List<GetEmployeeVM>> GetNotActiveEmployees();
    }
}
