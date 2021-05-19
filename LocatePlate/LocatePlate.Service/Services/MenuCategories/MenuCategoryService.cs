using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.MenuCategories;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.MenuCategories
{
    public class MenuCategoryService : BaseService<MenuCategory, IMenuCategoryRepository>, IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository)
            : base(menuCategoryRepository)
        {
        }
    }
}
