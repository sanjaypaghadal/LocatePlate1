using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;

namespace LocatePlate.Repository.Capactities
{
    public interface ICapacityRepository : IBaseRepository<Capacity>
    {
        IEnumerable<Capacity> GetAllByUserAndRestaurant(Guid userId, int restaurantId);
        List<Capacity> GetCapacityByRestaurantAndPartySize(int RestaurantId, int partySize);
        List<Capacity> GetAllByRestaurantId(int restaurantId);
        List<Capacity> GetCapacityByRestaurantPartySizeAndArea(int RestaurantId, int partySize, Area area);
    }
}
