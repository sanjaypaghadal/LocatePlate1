using LocatePlate.Model.Cms;
using LocatePlate.Repository.Repositories.Abstract;

namespace LocatePlate.Repository.PageLayouts
{
    public class PageLayoutRepositoryMongoDB : BaseRepositoryMongoDB<PageLayout>, IPageLayoutRepositoryMongoDB
    {
        public PageLayoutRepositoryMongoDB(ILocatePlateMongoDBContext<PageLayout> locatePlateMongoDBContext)
            : base(locatePlateMongoDBContext)
        {
        }
    }
}
