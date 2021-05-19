using LocatePlate.Model.Cms;
using LocatePlate.Repository.Sections;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.Sections
{
    public class SectionService : BaseServiceMongoDB<Section, ISectionRepositoryMongoDB>, ISectionService
    {
        private readonly ISectionRepositoryMongoDB _sectionRepository;
        public SectionService(ISectionRepositoryMongoDB sectionRepository)
            : base(sectionRepository)
        {
        }
    }
}
