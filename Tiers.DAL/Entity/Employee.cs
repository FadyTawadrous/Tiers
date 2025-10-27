namespace Tiers.DAL.Entity
{
    public class Employee
    {
        protected Employee() { } // For EF Core Reflection
        public Employee(string name, decimal salary, int age, string imageUrl, int departmentId, string createdUser )
        {
            Name = name;
            Salary = salary;
            CreatedBy = createdUser;
            Age = age;
            ImageUrl = imageUrl;
            CreatedOn = DateTime.Now;
            DepartmentId = departmentId;
            IsDeleted = false;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public decimal Salary { get; private set; }
        public string? ImageUrl { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        public int DepartmentId { get; private set; }
        public virtual Department Department { get; private set; }
        public bool Update(string name, decimal salary, int departmentId, string userModified)
        {
            if (!userModified.IsNullOrEmpty())
            {
                Name = name;
                Salary = salary;
                UpdatedOn = DateTime.Now;
                UpdatedBy = userModified;
                DepartmentId = departmentId;
                return true;
            }
            return false;
        }
        public bool ToggleDelete(string userModified)
        {
            if (!userModified.IsNullOrEmpty())
            {
                IsDeleted = !IsDeleted;
                DeletedOn = DateTime.Now;
                DeletedBy = userModified;
                return true;
            }
            return false;
        }
    }
}
