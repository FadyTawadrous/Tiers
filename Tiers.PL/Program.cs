using Microsoft.EntityFrameworkCore;
using Tiers.DAL.Common;
using Tiers.BLL.Common;
using Tiers.DAL.Database;
using Tiers.DAL.Entity;

namespace Tiers.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Database Context Configuration
            //var connectionString = builder.Configuration.GetConnectionString("TemplateConnection");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("MyDB"));

            //builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            //builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();

            builder.Services.AddBusinessInDAL();
            builder.Services.AddBusinessInBLL();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seeding initial data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                context.Employees.AddRange(
                    new Employee ( "Fady", 20000, "Admin" ),
                    new Employee("Ahmed", 46000, "Admin2")

                );
                context.SaveChanges();
            }


            app.Run();
        }
    }
}
