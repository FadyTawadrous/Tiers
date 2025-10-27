
namespace Tiers.BLL.ModelVM.Employee
{
    public class DeleteEmployeeVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DeletedBy { get; set; } = string.Empty;

    }
}
