using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Timings;
using LocatePlate.Service.Abstract;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Timings
{
    public class TimingService : BaseService<Timing, ITimingRepository>, ITimingService
    {
        private readonly ITimingRepository _timingRepository;
        public TimingService(ITimingRepository timingRepository)
            : base(timingRepository)
        {
            this._timingRepository = timingRepository;
        }

        public IEnumerable<Timing> GetAllByUserAndRestaurant(Guid userId, int restaurantId)
        {
            return this._timingRepository.GetAllByUserAndRestaurant(userId, restaurantId);
        }

        public Timing GetCurrentDayTime(int restaurantId, DateTime date) => this._timingRepository.GetCurrentDayTime(restaurantId, date);
    }
}
