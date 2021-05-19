using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Repositories.Discounts
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        private readonly LocatePlateContext _context;
        public DiscountRepository(LocatePlateContext context) : base(context)
        {
            this._context = context;
        }

        public List<Discount> GetByUrl(string url) => this._context.Discounts.Where(c => c.LocationUrl.ToLower() == url.ToLower()).ToList();
        public List<Discount> GetByUrl(string url, List<string> excludeDeals) => this._context.Discounts.Where(c => c.LocationUrl == url && c.IsCustom == false && !excludeDeals.Contains(c.PromoCode)).ToList();
        public Discount GetDiscountByCodeAndRestaurantId(string promocode, int restaurantId) => this._context.Discounts.FromSqlRaw("EXECUTE [dbo].[GetDiscountByCodeAndRestaurantId] {0},{1}", promocode, restaurantId).ToList().FirstOrDefault();
    }
}
