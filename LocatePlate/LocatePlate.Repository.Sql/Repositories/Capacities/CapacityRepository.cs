using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Capactities
{
    public class CapacityRepository : BaseRepository<Capacity>, ICapacityRepository
    {
        private readonly LocatePlateContext _locatePlateContext;
        public CapacityRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public IEnumerable<Capacity> GetAllByUserAndRestaurant(Guid userId, int restaurantId)
        {
            return this._locatePlateContext.Capacities.Where(c => c.UserId == userId && c.RestaurantId == restaurantId).AsEnumerable();
        }
        public List<Capacity> GetCapacityByRestaurantAndPartySize(int RestaurantId, int partySize) =>
             this._locatePlateContext.Capacities.Where(c => c.RestaurantId == RestaurantId && c.Size == partySize).ToList();
        public List<Capacity> GetAllByRestaurantId(int restaurantId)
        {
            var capacity = this._locatePlateContext.Capacities.Where(c => c.RestaurantId == restaurantId && c.Size == c.Size).AsEnumerable();
            return DistinctBy(capacity, p => p.Size).ToList();
        }
        public List<Capacity> GetCapacityByRestaurantPartySizeAndArea(int RestaurantId, int partySize, Area area) =>
         this._locatePlateContext.Capacities.Where(c => c.RestaurantId == RestaurantId && c.Size == partySize && c.Area == area).ToList();
    }
}
