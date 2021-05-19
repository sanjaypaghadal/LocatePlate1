using LocatePlate.Model.Cms;
using LocatePlate.Model.Cms.Modules;
using LocatePlate.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Security.Authentication;

namespace LocatePlate.WebApi.Installers
{
    public class MongoDbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            // Intilize the mongodb client
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration.GetConnectionString("LocatePlateMongoDbContext")));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);


            // Get mongo databases.
            var userManagerDatabase = configuration.GetValue<string>("DataBases:LocatePlate");

            //  Register mongo context.
            services.AddTransient<ILocatePlateMongoDBContext<Page>>(
                x => new LocatePlateMongoDBContext<Page>(mongoClient, userManagerDatabase));
            services.AddTransient<ILocatePlateMongoDBContext<PageLayout>>(
              x => new LocatePlateMongoDBContext<PageLayout>(mongoClient, userManagerDatabase));
            services.AddTransient<ILocatePlateMongoDBContext<Section>>(
              x => new LocatePlateMongoDBContext<Section>(mongoClient, userManagerDatabase));
            services.AddTransient<ILocatePlateMongoDBContext<Module>>(
              x => new LocatePlateMongoDBContext<Module>(mongoClient, userManagerDatabase));
        }
    }
}
