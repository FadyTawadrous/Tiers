using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiers.BLL.ModelVM.Department
{
    public class CreateDepartmentVM
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public required string Area { get; set; } = string.Empty;
        [Required]
        public required string CreatedBy { get; set; } = string.Empty;
    }
}
