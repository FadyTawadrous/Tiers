using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiers.BLL.ModelVM.Employee;
using Tiers.BLL.Responses;

namespace Tiers.BLL.Service.Abstraction
{
    public class IDepartmentService
    {
        Task<ResponseResult<List<GetEmployeeVM>>> GetActiveEmployees();
        Task<ResponseResult<List<GetEmployeeVM>>> GetNotActiveEmployees();
    }
}
