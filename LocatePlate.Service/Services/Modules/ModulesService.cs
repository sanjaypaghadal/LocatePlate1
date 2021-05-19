using LocatePlate.Model.Cms.Modules;
using LocatePlate.Repository.Modules;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.Services.Modules
{
    public class ModuleService : BaseServiceMongoDB<Module, IModuleRepositoryMongoDB>, IModuleService
    {
        private readonly IModuleRepositoryMongoDB _moduleRepository;
        public ModuleService(IModuleRepositoryMongoDB moduleRepository)
            : base(moduleRepository)
        {
        }
    }
}
