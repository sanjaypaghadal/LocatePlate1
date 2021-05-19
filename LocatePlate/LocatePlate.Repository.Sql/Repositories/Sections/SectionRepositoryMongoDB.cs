using LocatePlate.Model.Cms;
using LocatePlate.Repository.Repositories.Abstract;

namespace LocatePlate.Repository.Sections
{
    public class SectionRepositoryMongoDB : BaseRepositoryMongoDB<Section>, ISectionRepositoryMongoDB
    {
        public SectionRepositoryMongoDB(ILocatePlateMongoDBContext<Section> locatePlateMongoDBContext)
            : base(locatePlateMongoDBContext)
        {
        }
    }
}
