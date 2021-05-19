using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LocatePlate.WebApi.Installers
{
    public class SqlDbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LocatePlateContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LocatePlateSqlContext"),
                    b => b.MigrationsAssembly("LocatePlate.Repository")))
                .AddDefaultIdentity<UserIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<LocatePlateContext>();
        }
    }
}
