using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Capactities
{
    public interface ICapacityService : IBaseService<Capacity>
    {
        IEnumerable<Capacity> GetAllByUserAndRestaurant(Guid userId, int restaurantId);
        List<Capacity> GetCapacityByRestaurantAndPartySize(int RestaurantId, int partySize);
        List<Capacity> GetAllByRestaurantId(int restaurantId);
    }
}
