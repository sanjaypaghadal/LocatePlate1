using LocatePlate.Infrastructure.Domain;
using LocatePlate.WebApi.DependencyResolution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocatePlate.WebApi.Installers
{
    public class AppsettingsIntsaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register project dependencies
            services.RegisterDependencies();
            services.Configure<Host>(configuration.GetSection("Host"));
        }
    }
}
