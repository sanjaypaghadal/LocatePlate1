using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;

namespace LocatePlate.Repository.Menus
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        List<Menu> GetByCategoryId(int catid);
        PaginatedList<Menu> GetMenuList(Guid userId, int restaurantId, int page, int pageSize);
    }
}
