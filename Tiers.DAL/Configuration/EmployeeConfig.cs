using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiers.DAL.Configuration
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Salary)
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.ImageUrl)
                   .HasMaxLength(255);

            builder.Property(e => e.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.CreatedOn)
                   .IsRequired();

            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
