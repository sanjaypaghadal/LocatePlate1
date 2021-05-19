using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Repositories.Discounts;
using LocatePlate.Service.Abstract;
using System.Collections.Generic;

namespace LocatePlate.Service.Services.Discounts
{
    public class DiscountService : BaseService<Discount, IDiscountRepository>, IDiscountService
    {
        public IDiscountRepository _discountRepository { get; set; }
        public DiscountService(IDiscountRepository discountRepository) : base(discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public List<Discount> GetByUrl(string url) => this._discountRepository.GetByUrl(url);
        public List<Discount> GetByUrl(string url, List<string> excludeDeals) => this._discountRepository.GetByUrl(url, excludeDeals);
        public Discount GetDiscountByCodeAndRestaurantId(string promocode, int restaurantId) => this._discountRepository.GetDiscountByCodeAndRestaurantId(promocode, restaurantId);
    }
}
