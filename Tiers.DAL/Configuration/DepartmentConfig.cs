using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tiers.DAL.Configuration
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Area)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(d => d.CreatedOn)
                   .IsRequired();

            builder.HasMany(d => d.Employees)
                   .WithOne(e => e.Department)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
