using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;

namespace LocatePlate.Repository.MenuCategories
{
    public class MenuCatergoryRepository : BaseRepository<MenuCategory>, IMenuCategoryRepository
    {
        public MenuCatergoryRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {

        }
    }
}
