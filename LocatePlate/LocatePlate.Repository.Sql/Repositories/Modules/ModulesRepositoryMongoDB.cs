using LocatePlate.Model.Cms.Modules;
using LocatePlate.Repository.Repositories.Abstract;

namespace LocatePlate.Repository.Modules
{
    public class ModuleRepositoryMongoDB : BaseRepositoryMongoDB<Module>, IModuleRepositoryMongoDB
    {
        public ModuleRepositoryMongoDB(ILocatePlateMongoDBContext<Module> locatePlateMongoDBContext)
            : base(locatePlateMongoDBContext)
        {
        }
    }
}
