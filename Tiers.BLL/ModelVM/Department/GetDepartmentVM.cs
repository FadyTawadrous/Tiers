using Tiers.BLL.ModelVM.Employee;

namespace Tiers.BLL.ModelVM.Department
{
    public class GetDepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }
}
