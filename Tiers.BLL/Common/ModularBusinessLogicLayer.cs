using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiers.BLL.Service.Abstraction;
using Tiers.BLL.Service.Implementation;
using Tiers.DAL.Repo.Abstraction;
using Tiers.DAL.Repo.Implementation;

namespace Tiers.BLL.Common
{
    public static class ModularBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessInBLL(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
