using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Capactities;
using LocatePlate.Service.Abstract;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Capactities
{
    public class CapacityService : BaseService<Capacity, ICapacityRepository>, ICapacityService
    {
        private readonly ICapacityRepository _capacityRepository;
        public CapacityService(ICapacityRepository capacityRepository) : base(capacityRepository)
        {
            this._capacityRepository = capacityRepository;
        }

        public IEnumerable<Capacity> GetAllByUserAndRestaurant(Guid userId, int restaurantId)
        {
            return this._capacityRepository.GetAllByUserAndRestaurant(userId, restaurantId);
        }

        public List<Capacity> GetCapacityByRestaurantAndPartySize(int RestaurantId, int partySize) => this._capacityRepository.GetCapacityByRestaurantAndPartySize(RestaurantId, partySize);
        public List<Capacity> GetAllByRestaurantId(int restaurantId) => this._capacityRepository.GetAllByRestaurantId(restaurantId);
    }
}
