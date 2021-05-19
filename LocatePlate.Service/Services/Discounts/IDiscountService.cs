using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System.Collections.Generic;

namespace LocatePlate.Service.Services.Discounts
{
    public interface IDiscountService : IBaseService<Discount>
    {
        List<Discount> GetByUrl(string url);
        List<Discount> GetByUrl(string url, List<string> excludeDeals);
        Discount GetDiscountByCodeAndRestaurantId(string promocode, int restaurantId);
    }
}
