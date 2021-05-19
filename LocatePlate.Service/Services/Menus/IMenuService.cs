using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Service.Menus
{
    public interface IMenuService : IBaseService<Menu>
    {
        List<Menu> GetByCategoryId(int catid);

        public Task<int> deleteMenu(int menuId);
        public PaginatedList<Menu> GetMenuList(Guid userId, int restaurantId, int page, int pageSize);
    }
}
