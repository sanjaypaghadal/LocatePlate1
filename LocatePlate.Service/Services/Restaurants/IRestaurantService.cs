using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;

namespace LocatePlate.Service.Restaurants
{
    public interface IRestaurantService : IBaseService<Restaurant>
    {
        List<Restaurant> GetByUser(Guid userId);
        PaginatedList<Restaurant> GetByLocation(int page, int pageSize, Guid locationid);
        Restaurant GetByUrl(string url);
        Restaurant GetDetailByUrl(string url, string locationName);
        Restaurant GetDetailById(int restaurantId);
        Ratings GiveRating(Ratings rating);
        List<Restaurant> GetNewRestaurants(int take, Guid locationId);
    }
}
