using Tiers.BLL.ModelVM.Employee;

namespace Tiers.BLL.ModelVM.Department
{
    public class GetDepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public ICollection<GetEmployeeVM> Employees { get; set; }
    }
}
