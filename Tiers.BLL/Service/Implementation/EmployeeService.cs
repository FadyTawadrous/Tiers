using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiers.BLL.ModelVM.Employee;
using Tiers.BLL.Responses;
using Tiers.BLL.Service.Abstraction;
using Tiers.DAL.Repo.Abstraction;

namespace Tiers.BLL.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _employeeRepo;
        public EmployeeService(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        public ResponseResult<List<GetEmployeeVM>> GetActiveEmployees()
        {
            try
            {
                var result = _employeeRepo.GetAll(e => e.IsDeleted == false).ToList();
                var mappedResult = result.Select(e => new GetEmployeeVM
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList();

                return new ResponseResult<List<GetEmployeeVM>>(mappedResult, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<List<GetEmployeeVM>>(null, ex.Message, false);
            }
        }

        public ResponseResult<List<GetEmployeeVM>> GetNotActiveEmployees()
        {
            try
            {
                var result = _employeeRepo.GetAll(e => e.IsDeleted == true).ToList();
                var mappedResult = result.Select(e => new GetEmployeeVM
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList();

                return new ResponseResult<List<GetEmployeeVM>>(mappedResult, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<List<GetEmployeeVM>>(null, ex.Message, false);
            }
        }
    }
}
