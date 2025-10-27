using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiers.DAL.Repo.Abstraction;
using Tiers.DAL.Repo.Implementation;

namespace Tiers.DAL.Common
{
    public static class ModularDataAccessLayer
    {
        public static IServiceCollection AddBusinessInDAL(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            return services;
        }
    }
}
