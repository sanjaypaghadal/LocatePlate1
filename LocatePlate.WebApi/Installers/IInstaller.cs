using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocatePlate.WebApi.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
