using LocatePlate.Model.Cms;
using LocatePlate.Repository.PageLayouts;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.PagesLayouts
{
    public class PageLayoutService : BaseServiceMongoDB<PageLayout, IPageLayoutRepositoryMongoDB>, IPageLayoutService
    {
        private readonly IPageLayoutRepositoryMongoDB _pageLayoutSectionRepository;
        public PageLayoutService(IPageLayoutRepositoryMongoDB pageLayoutSectionRepository)
            : base(pageLayoutSectionRepository)
        {
        }
    }
}
