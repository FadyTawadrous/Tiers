using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiers.DAL.Configuration
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee").HasKey(e => e.Id);
        }
    }
}
