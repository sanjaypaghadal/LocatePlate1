using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Menus
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        private readonly LocatePlateContext _locatePlateContext;
        public MenuRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public List<Menu> GetByCategoryId(int catid) => this._locatePlateContext.Menus.Where(c => c.MenuCategoryId == catid).ToList();

        public PaginatedList<Menu> GetMenuList(Guid userId, int restaurantId, int page, int pageSize)
        {
            var count = _locatePlateContext.Menus.Where(m => m.UserId == userId && m.RestaurantId == restaurantId).Count();

            var menus = (from m in _locatePlateContext.Menus
                         join mr in _locatePlateContext.MenuReservation on m.Id equals mr.MenuId into mrleft
                         from mrl in mrleft.DefaultIfEmpty()
                         where m.UserId == userId && m.RestaurantId == restaurantId
                         select new Menu
                         {
                             Id = m.Id,
                             Name = m.Name,
                             Price = m.Price,
                             Images = m.Images,
                             About = m.About,
                             Recipes = m.Recipes,
                             FoodNature = m.FoodNature,
                             RestaurantId = m.RestaurantId,
                             MenuCategoryId = m.MenuCategoryId,
                             CreatedBy = m.CreatedBy,
                             ModifiedBy = m.ModifiedBy,
                             CreatedDate = m.CreatedDate,
                             ModifiedDate = m.ModifiedDate,
                             IsSoftDelete = m.IsSoftDelete,
                             UserId = m.UserId,
                             Calories = m.Calories,
                             MenuCategoryName = m.MenuCategoryName,
                             FoodNatureName = m.FoodNatureName,
                             RestaurantName = m.RestaurantName,
                             IsMenuReservation = (mrl != null ? true : false)
                         }).OrderBy(m => m.Id).Skip(page * pageSize).Take(pageSize).ToList();

            var menupage = new PaginatedList<Menu>(menus.AsQueryable(), page, pageSize, count, (int)Math.Ceiling(count / (double)pageSize));

            return menupage;
        }
    }
}
