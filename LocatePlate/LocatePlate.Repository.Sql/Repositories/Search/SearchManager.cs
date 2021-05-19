using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;
using LocatePlate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LocatePlate.Repository.Repositories.Search
{
    public class SearchManager : ISearchManager
    {
        private readonly LocatePlateContext _locatePlateContext;
        public SearchManager(LocatePlateContext locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }
        public SearchRecords Search(SearchQuery searchQuery)
        {
            var resultSet = this._locatePlateContext.SearchRecords.FromSqlRaw(
                "EXECUTE [dbo].[LocatePlateSearch] {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                searchQuery.query == "" ? null : searchQuery.query,
                searchQuery.DealUrl == "" ? null : searchQuery.DealUrl,
                searchQuery.Tags == "" ? null : searchQuery.Tags,
                searchQuery.location,
                searchQuery.Cuisine == "" ? null : searchQuery.Cuisine,
                searchQuery.Categories == "" ? null : searchQuery.Categories,
                searchQuery.FoodType == "" ? null : searchQuery.FoodType,
                searchQuery.PageNumber == 0 ? 1 : searchQuery.PageNumber,
                searchQuery.RowsOfPage == 0 ? 10 : searchQuery.RowsOfPage,
                searchQuery.SortingCol,
                searchQuery.SortType= searchQuery.SortingCol == "1"?1: searchQuery.SortType,
                ((searchQuery.SortingCol == "2" || searchQuery.SortingCol == "3") && searchQuery.Latitude != 0) ? searchQuery.Latitude : null,
                ((searchQuery.SortingCol == "2" || searchQuery.SortingCol == "3") && searchQuery.Logitude != 0) ? searchQuery.Logitude : null,
                searchQuery.PartySize,
                searchQuery.SeatingPreference = searchQuery.SeatingPreference // should be -1 if not selected any preference
                ).ToList();
            var restaurants = resultSet.ToList();
            var restids = restaurants.Select(c => c.RestaurantId).ToList();
            var dish = resultSet.Where(c => c.Dish != null).SelectMany(c => c.Dish?.ToLower()?.Split(',')?.ToList()).ToList();
            var foodCategory = resultSet.Where(c => c.FoodCategory != null).SelectMany(c => c.FoodCategory?.ToLower()?.Split(',')?.ToList()).ToList();
            var FoodType = resultSet.Where(c => c.FoodType != null).SelectMany(c => c.FoodType?.ToLower()?.Split(',')?.ToList()).ToList();

            return new SearchRecords
            {
                Dish = dish,
                FoodCategory = foodCategory,
                FoodType = FoodType,
                SearchResults = restaurants
            };
        }

        public CurrentLocation GetNearByLocation(SearchQuery searchQuery)
        {
            var resultSet = this._locatePlateContext.CurrentLocation.FromSqlRaw(
                "EXECUTE [dbo].[NearByCity] {0},{1}", searchQuery.Latitude.Value, searchQuery.Logitude.Value)?.ToList()?.FirstOrDefault();

            return resultSet;
        }

        // not in use now
        public SearchRecords SearchDeals(SearchQuery searchQuery)
        {
            var resultSet = this._locatePlateContext.SearchRecords.FromSqlRaw("EXECUTE [dbo].[LocatePlateSearchDeals] {0},{1},{2},{3},{4},{5},{6},{7},{8}", searchQuery.query, searchQuery.location, searchQuery.Cuisine == "" ? null : searchQuery.Cuisine, searchQuery.Categories == "" ? null : searchQuery.Categories, searchQuery.FoodType == "" ? null : searchQuery.FoodType, searchQuery.PageNumber, searchQuery.RowsOfPage, searchQuery.SortingCol, searchQuery.SortType).ToList();
            return new SearchRecords
            {
                SearchResults = resultSet
            };
        }

        // not in use now
        public SearchRecords SearchTags(SearchQuery searchQuery)
        {
            var resultSet = this._locatePlateContext.SearchRecords.FromSqlRaw("EXECUTE [dbo].[LocatePlateSearchTags] {0},{1},{2},{3},{4},{5}", searchQuery.query, searchQuery.location, searchQuery.PageNumber, searchQuery.RowsOfPage, searchQuery.SortingCol, searchQuery.SortType).ToList();
            return new SearchRecords
            {
                SearchResults = resultSet
            };
        }

        public FilterCriteria GetFilterData()
        {
            var result = new FilterCriteria();
            result.Cuisine = this._locatePlateContext.StringRecords.FromSqlRaw("EXECUTE [dbo].[GetAllCuisine]").ToList();
            //  foreach (MealType item in Enum.GetValues(typeof(MealType))) if (MealType.NotSelected != item) result.MealType.Add(new StringModel { Name = item.GetAttribute<DisplayAttribute>().Name });
            foreach (ResturantType item in Enum.GetValues(typeof(ResturantType))) if (ResturantType.NotSelected != item) result.RestaurantType.Add(new StringModel { Name = item.GetAttribute<DisplayAttribute>().Name });
            foreach (FoodNature item in Enum.GetValues(typeof(FoodNature))) result.FoodNature.Add(new StringModel { Name = item.GetAttribute<DisplayAttribute>().Name });
            return result;
        }
    }
}
