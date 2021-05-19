using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Restaurants
{
    public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
    {
        protected readonly LocatePlateContext _locatePlateContext;
        public RestaurantRepository(LocatePlateContext locatePlateContext)
            : base(locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public List<Restaurant> GetByUser(Guid userId)
        {
            return this._locatePlateContext.Restaurants.Where(c => c.UserId == userId && c.IsSoftDelete != true).ToList();
        }

        public PaginatedList<Restaurant> GetByLocation(int page, int pageSize, Guid locationid)
        {
            var result = this._locatePlateContext.Restaurants.Include(c => c.Menus)
                .Include(c => c.Timings)
                .Where(c => (c.Menus != null && c.Menus.Any()) && (c.Timings != null && c.Timings.Any()) && c.LocationId == locationid).OrderByDescending(c => c.CreatedDate);
            var paginatedResult = new PaginatedList<Restaurant>(result, page, pageSize);

            return paginatedResult;
        }

        public Restaurant GetByUrl(string url)
        {
            var result = this._locatePlateContext.Restaurants.FirstOrDefault(c => c.Url.ToLower().Equals(url.ToLower()));

            return result;
        }

        public Restaurant GetDetailByUrl(string url, string locationName)
        {
            var resultSet = this._locatePlateContext.RestaurantStoreProcedure.FromSqlRaw("EXECUTE [dbo].[RestaurantDetails] {0},{1}", url, locationName).ToList();
            return FillStoreProcedureResultSetToRestaurant(resultSet);
        }

        public Restaurant GetDetailById(int restaurantId)
        {
            var resultSet = this._locatePlateContext.RestaurantStoreProcedure.FromSqlRaw("EXECUTE [dbo].[RestaurantDetailsById] {0}", restaurantId).ToList();
            return FillStoreProcedureResultSetToRestaurant(resultSet);
        }

        public List<Ratings> GetRating(int restaurantId)
        {
            return this._locatePlateContext.Ratings.Where(c => (c.RestaurantId == restaurantId))?.ToList();
        }

        //public override IEnumerable<Restaurant> GetAllByIds(List<int> ids)
        //{
        //    var result = from rest in _locatePlateContext.Restaurants
        //                 join rating in _locatePlateContext.Ratings on rest.Id equals rating.RestaurantId
        //                 into ratings
        //                 from y in ratings.DefaultIfEmpty()
        //                 join re in _locatePlateContext.Reviews on rest.Id equals re.RestaurantId
        //                 into reviews
        //                 from x in reviews.DefaultIfEmpty()
        //                 select new { Restaurants = rest, Ratings = ratings, Reviews = x };


        //    var rss = (from rest2 in _locatePlateContext.Restaurants
        //               from co in _locatePlateContext.Ratings.Where(co => co.RestaurantId == rest2.Id).DefaultIfEmpty()
        //               from prod in _locatePlateContext.Reviews.Where(prod => prod.RestaurantId == rest2.Id).DefaultIfEmpty()
        //               select new { Person = rest2, Company = co, Product = prod });


        //    //   select new { person.FirstName, PetName = subpet?.Name ?? String.Empty };
        //    try
        //    {

        //        foreach (var item in rss)
        //        {

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    //var rests = this._locatePlateContext.Restaurants.Where(s => ids.Contains(s.Id)).AsEnumerable();
        //    //var ratings = GetRatingByRestaurantIds(ids);
        //    //foreach (var item in result)
        //    //{
        //    //    item.Restaurants.RatingCount = item.Ratings.Average(c => c.Rating);
        //    //    item.Restaurants.ReviewCount = item.Reviews.;

        //    //    var restaurants = ratings.Where(c => c.RestaurantId == item.Id).ToList();
        //    //    if (restaurants.Any())
        //    //        item.RatingCount = restaurants.Average(a => a.Rating);
        //    //}

        //    return (IEnumerable<Restaurant>)result.FirstOrDefault().Restaurants;
        //}

        public List<Ratings> GetRatingByRestaurantIds(List<int> restaurantIds)
        {
            var result = this._locatePlateContext.Ratings.Where(s => restaurantIds.Contains(s.RestaurantId)).ToList();
            return result;
        }

        public List<Reviews> GetReviewsByRestaurantIds(List<int> restaurantIds)
        {
            var result = this._locatePlateContext.Reviews.Where(s => restaurantIds.Contains(s.RestaurantId)).ToList();
            return result;
        }

        public Ratings GiveRating(Ratings rating)
        {
            var IsAlready = this._locatePlateContext.Ratings.FirstOrDefault(c => (c.UserId == rating.UserId) && (c.RestaurantId == rating.RestaurantId) && (c.RatingType == rating.RatingType));
            if (IsAlready != null)
            {
                IsAlready.Rating = rating.Rating;
                this._locatePlateContext.Ratings.Update(IsAlready);
                this._locatePlateContext.SaveChanges();
            }
            else
            {
                this._locatePlateContext.Ratings.Add(rating);
                this._locatePlateContext.SaveChanges();
            }

            return rating;
        }

        public override Restaurant Delete(Restaurant entity)
        {
            //var result = this._locatePlateContext.Restaurants.FromSqlRaw("EXECUTE [dbo].[DeleteRestaurant] @Id={0}", entity.Id.ToString());
            entity.IsSoftDelete = true;
            this._locatePlateContext.Update(entity);
            this._locatePlateContext.SaveChanges();
            return entity;
        }

        public List<Restaurant> GetNewRestaurants(int take, Guid locationId)
        {
            List<Restaurant> newRestaurants = this._locatePlateContext.Restaurants.Include(c => c.Menus).Include(c => c.Timings).Where(c => (c.Menus != null && c.Menus.Any()) && (c.Timings != null && c.Timings.Any()) && c.LocationId == locationId).Take(take).OrderByDescending(c => c.CreatedDate).ToList();
            return newRestaurants;
        }

        private Restaurant FillStoreProcedureResultSetToRestaurant(List<RestaurantStoreProcedure> resultSet)
        {
            if (resultSet.Count == 0)
                return new Restaurant();

            var rest = resultSet.FirstOrDefault();
            var restaurant = new Restaurant
            {
                Id = rest.RestaurantId,
                Name = rest.RestaurantName,
                About = rest.About,
                Cuisine = rest.Cuisine,
                FullAddress = rest.FullAddress,
                CostForTwo = rest.CostForTwo,
                RatingCount = rest.RatingCount,
                ReviewCount = rest.ReviewCount,
                CoverImages = rest.CoverImages,
                UserId = rest.UserId,
                Latitude = rest.Latitude,
                Longitude = rest.Longitude,
                LocationId = rest.LocationId
            };

            restaurant = FillRating(restaurant);
            restaurant.Menus = new List<Menu>();
            resultSet.ForEach(c => restaurant.Menus.Add(new Menu { UserId = rest.UserId, RestaurantId = rest.RestaurantId, Id = c.MenuId, Name = c.MenuItem, Price = c.Price, Calories = c.Calories, FoodNatureName = c.FoodNatureName, Images = c.Images, MenuCategoryName = c.MenuCategoryName, About = c.MenuAbout, Quantity = 0 }));
            restaurant.Timings = this._locatePlateContext.Timings.Where(c => c.RestaurantId == rest.RestaurantId).ToList();
            return restaurant;
        }

        private Restaurant FillRating(Restaurant restaurant)
        {
            restaurant.Ratings = GetRating(restaurant.Id);
            if (restaurant.Ratings?.Count() != null)
                restaurant.RatingTotalCount = restaurant.Ratings?.Count();

            restaurant.RatingGroups = restaurant.Ratings?.GroupBy(c => c.RatingType).Select(group =>
                                                                        new RatingGroups
                                                                        {
                                                                            Key = group.Key.ToString(),
                                                                            Value = group.ToList(),
                                                                            Avg = group.Average(c => c.Rating),
                                                                            AvgPercentage = ((group.Sum(c => c.Rating) / 5) * 100)
                                                                        }).OrderBy(x => x.Key).ToList();

            return restaurant;
        }
    }
}
