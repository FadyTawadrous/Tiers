using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiers.DAL.Entity
{
    public class Department
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Area { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        public virtual ICollection<Employee>? Employees { get; private set; }
        public Department() { }
        public Department(string name, string area, string createdBy)
        {
            Name = name;
            Area = area;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
        }

        public bool Update(string name, string area, string userModified)
        {
            if (!userModified.IsNullOrEmpty())
            {
                Name = name;
                Area = area;
                UpdatedOn = DateTime.Now;
                UpdatedBy = userModified;
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
