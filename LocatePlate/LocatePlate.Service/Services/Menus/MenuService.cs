using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Menus;
using LocatePlate.Repository.Repositories.DBConnection;
using LocatePlate.Service.Abstract;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Service.Menus
{
    public class MenuService : BaseService<Menu, IMenuRepository>, IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IConnection _connection;
        public MenuService(IMenuRepository menuRepository, IConnection connection)
            : base(menuRepository)
        {
            this._menuRepository = menuRepository;
            _connection = connection;
        }

        public async Task<int> deleteMenu(int menuId)
        {
            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter("@menuId", menuId)
            };
            var result = Convert.ToInt32(await _connection.GetExecuteScalarSP("DeleteMenu", para));
            return result;
        }

        public List<Menu> GetByCategoryId(int catid) => this._menuRepository.GetByCategoryId(catid);

        public PaginatedList<Menu> GetMenuList(Guid userId, int restaurantId, int page, int pageSize) => this._menuRepository.GetMenuList(userId, restaurantId, page, pageSize);
    }
}
