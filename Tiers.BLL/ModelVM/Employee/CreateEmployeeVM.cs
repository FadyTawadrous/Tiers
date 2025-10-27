using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiers.BLL.ModelVM.Employee
{
    public class CreateEmployeeVM
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public required IFormFile Image { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public int DepartmentId { get;  set; }
    }
}
