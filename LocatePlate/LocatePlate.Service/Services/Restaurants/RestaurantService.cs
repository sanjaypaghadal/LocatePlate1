using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Restaurants;
using LocatePlate.Service.Abstract;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Restaurants
{
    public class RestaurantService : BaseService<Restaurant, IRestaurantRepository>, IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository)
            : base(restaurantRepository)
        {
            this._restaurantRepository = restaurantRepository;
        }

        public List<Restaurant> GetByUser(Guid userId)
        {
            return this._restaurantRepository.GetByUser(userId);
        }

        public PaginatedList<Restaurant> GetByLocation(int page, int pageSize, Guid locationid)
        {
            return this._restaurantRepository.GetByLocation(page, pageSize, locationid);
        }

        public Restaurant GetByUrl(string url) => this._restaurantRepository.GetByUrl(url);

        public Restaurant GetDetailByUrl(string url, string locationName) =>this._restaurantRepository.GetDetailByUrl(url, locationName);
        public Restaurant GetDetailById(int restaurantId) => this._restaurantRepository.GetDetailById(restaurantId);
        public Ratings GiveRating(Ratings rating) => this._restaurantRepository.GiveRating(rating);
        public List<Restaurant> GetNewRestaurants(int take,Guid locationId) => this._restaurantRepository.GetNewRestaurants(take, locationId);
    }
}
