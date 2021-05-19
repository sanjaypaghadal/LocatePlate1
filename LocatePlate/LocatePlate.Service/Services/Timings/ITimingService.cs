using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Timings
{
    public interface ITimingService : IBaseService<Timing>
    {
        IEnumerable<Timing> GetAllByUserAndRestaurant(Guid userId, int restaurantId);
        Timing GetCurrentDayTime(int restaurantId, DateTime date);
    }
}
