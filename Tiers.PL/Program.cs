using Microsoft.EntityFrameworkCore;
using Tiers.BLL.AutoMapper;
using Tiers.BLL.Common;
using Tiers.DAL.Common;
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

            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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

                if (!context.Departments.Any() && !context.Employees.Any())
                {
                    var hr = new Department("HR", "Cairo", "Fady");
                    var it = new Department("IT", "Alex", "Ahmed");

                    context.Departments.AddRange(hr, it);
                    context.SaveChanges();

                    context.Employees.AddRange(
                    new Employee("Fady", 20000, 30, "testpic.png", hr.Id, "Admin"),
                    new Employee("Ahmed", 46000, 28, "testpic2.JPG", it.Id, "Admin2")
                    );
                    context.SaveChanges();
                }
            }


            app.Run();
        }
    }
}
