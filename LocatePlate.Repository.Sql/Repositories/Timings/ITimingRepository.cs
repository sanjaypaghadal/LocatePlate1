using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;

namespace LocatePlate.Repository.Timings
{
    public interface ITimingRepository : IBaseRepository<Timing>
    {
        IEnumerable<Timing> GetAllByUserAndRestaurant(Guid userId, int restaurantId);
        Timing GetCurrentDayTime(int restaurantId, DateTime date);
    }
}
