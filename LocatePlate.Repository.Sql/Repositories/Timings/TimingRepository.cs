using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Timings
{
    public class TimingRepository : BaseRepository<Timing>, ITimingRepository
    {
        private readonly LocatePlateContext _locatePlateContext;
        public TimingRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public IEnumerable<Timing> GetAllByUserAndRestaurant(Guid userId, int restaurantId)
        {
            return this._locatePlateContext.Timings.Where(c => c.UserId == userId && c.RestaurantId == restaurantId).ToList();
        }

        public Timing GetCurrentDayTime(int restaurantId, DateTime date) => this._locatePlateContext.Timings.FirstOrDefault(c => c.RestaurantId == restaurantId && c.Day == date.DayOfWeek);
    }
}