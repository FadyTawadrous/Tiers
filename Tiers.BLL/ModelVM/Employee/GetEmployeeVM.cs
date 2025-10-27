using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiers.BLL.ModelVM.Employee
{
    public class GetEmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string? ImageUrl { get; set; }
        public int DepartmentId { get; set; }
    }
}
