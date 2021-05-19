using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LocatePlate.WebApi.Controllers.Website
{
    public partial class HomeController
    {

        [HttpGet("restaurants/search")]
        public IActionResult Search()
        {
            ViewData["Title"] = $"Search | LocatePlate";
            
            var searchResult = ExecuteSearch();
            SetCityName(ref searchResult);
            FillFilter();
            return View("RestaurantFilter", searchResult);
        }

        [HttpGet("restaurants/searchquery")]
        public IActionResult SearchQuery(string term)
        {
            var result = new List<SearchResultJson>();
            var searchResult = ExecuteSearch();
            SetCityName(ref searchResult);
            if (searchResult?.Dish?.Any() != null)
                for (int i = 0; i < searchResult.Dish.Count; i++)
                    if (searchResult.Dish[i].ToLower() == term.ToLower())
                        result.Add(new SearchResultJson { Href = $"/restaurants/search?query={term}", Name = searchResult.Dish[i], Icon = 0 });

            if (searchResult?.FoodCategory?.Any() != null)
                for (int i = 0; i < searchResult.FoodCategory.Count; i++)
                    if (searchResult.FoodCategory[i].ToLower() == term.ToLower())
                        result.Add(new SearchResultJson { Href = $"/restaurants/search?query={term}", Name = searchResult.FoodCategory[i], Icon = 1 });

            if (searchResult?.FoodType?.Any() != null)
                for (int i = 0; i < searchResult.FoodType.Count; i++)
                    if (searchResult.FoodType[i].ToLower() == term.ToLower())
                        result.Add(new SearchResultJson { Href = $"/restaurants/search?query={term}", Name = searchResult.FoodType[i], Icon = 2 });

            if (searchResult?.SearchResults?.Any() != null)
                for (int i = 0; i < searchResult.SearchResults.Count; i++)
                    result.Add(new SearchResultJson { Href = $"/{searchResult.SearchResults[i].RestaurantUrl}", Name = searchResult.SearchResults[i].Name, Icon = 3 });

            return Ok(result);
        }

        private void FillFilter()
        {
            var filtermeta = this._searchService.GetFilterData();
            ViewBag.Cuisine = filtermeta.Cuisine;
            ViewBag.FoodNature = filtermeta.FoodNature;
            ///ViewBag.MealType = filtermeta.MealType;
            ViewBag.RestaurantType = filtermeta.RestaurantType;
        }

        private SearchQuery GetQueryStringValues()
        {
            StringValues location;
            StringValues query;
            StringValues term;
            StringValues deals;
            StringValues tags;
            StringValues pagenumber;
            StringValues pagesize;
            StringValues SortingCol;
            StringValues SortType;
            StringValues Cuisine;
            //   StringValues MealType = string.Empty;
            StringValues FoodNature;
            StringValues RestaurantType;
            StringValues Guest;
            StringValues Date;
            StringValues Area;

            Request.Query.TryGetValue("Location", out location);
            Request.Query.TryGetValue("Query", out query);
            Request.Query.TryGetValue("Deals", out deals);
            Request.Query.TryGetValue("Tags", out tags);
            Request.Query.TryGetValue("PageNumber", out pagenumber);
            Request.Query.TryGetValue("RowsOfPage", out pagesize);
            Request.Query.TryGetValue("SortingCol", out SortingCol);
            Request.Query.TryGetValue("SortType", out SortType);
            Request.Query.TryGetValue("Cuisine", out Cuisine);
            Request.Query.TryGetValue("FoodNature", out FoodNature);
            Request.Query.TryGetValue("RestaurantType", out RestaurantType);
            Request.Query.TryGetValue("Guest", out Guest);
            Request.Query.TryGetValue("Date", out Date);
            Request.Query.TryGetValue("Area", out Area);
            Request.Query.TryGetValue("term", out term);

            if (!string.IsNullOrEmpty(term))
                query = term;

            var page = this._pageService.GetByUrl(location);

            int pagesizeInt = 10;
            int.TryParse(pagesize, out pagesizeInt);
            int pagenumberInt = 1;
            int.TryParse(pagenumber, out pagenumberInt);
            int SortTypeInt = 0;
            int.TryParse(SortType, out SortTypeInt);

            var SearchDate = string.IsNullOrEmpty(Date) ? DateTime.Now : Convert.ToDateTime(Date);
            //if (this._ClientSide.ClientTime == DateTime.MinValue)
            //    this._ClientSide.ClientTime = DateTime.UtcNow;



            var dateTimeUnspec = DateTime.SpecifyKind(SearchDate, DateTimeKind.Unspecified);
            var utcdate = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, this._ClientSide.ClientInfo);
            if (utcdate.Date != SearchDate.Date) utcdate = SearchDate;

            //return new SearchQuery { Cuisine=Cuisine, MealType=MealType, FoodType=FoodNature, Categories= RestaurantType, RowsOfPage = 10, PageNumber = 1, SortingCol = "", SortType = 0, SearchDate = _ClientSide.ClientTime, ClientInfo = _ClientSide.ClientInfo };
            var searchQuery = new SearchQuery
            {
                //location = page?.Id,
                location = new Guid(location),
                query = query,
                DealUrl = deals,
                Tags = tags,
                Cuisine = Cuisine,
                FoodType = FoodNature,
                Categories = RestaurantType,
                RowsOfPage = pagesizeInt,
                PageNumber = pagenumberInt,
                SortingCol = SortingCol,
                SortType = SortTypeInt,
                SearchDate = utcdate,
                ClientInfo = _ClientSide.ClientInfo,
                PartySize = Convert.ToInt32(Guest == "" ? "0" : Guest),
                SeatingPreference = string.IsNullOrEmpty(Area) ? -1 : Convert.ToInt32(Area)
            };

            ///searchQuery.SearchDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(searchQuery.SearchDate, DateTimeKind.Unspecified), this._ClientSide.ClientInfo);
            var logitude = "";
            var latitude = "";
            getUserLocationfromCookie(ref logitude, ref latitude);
            if (!string.IsNullOrEmpty(logitude) && !string.IsNullOrEmpty(latitude))
            {
                searchQuery.Latitude = Convert.ToDecimal(latitude);
                searchQuery.Logitude = Convert.ToDecimal(logitude);
            }

            return searchQuery;
        }

        [HttpGet("restaurants/searchpartial")]
        public IActionResult SearchPartial()
        {
            var searchResult = ExecuteSearch();
            if (searchResult.SearchResults.Count == 0)
                return null;
            else
                return View("_RestaurantsFilterPartial", searchResult.SearchResults);
        }
        private SearchRecords ExecuteSearch()
        {
            var searchQuery = GetQueryStringValues();
            if (searchQuery.location.GetValueOrDefault() == Guid.Empty)
            {
                var Locationid = "";
                getLocationIdFromCookie(ref Locationid);
                searchQuery.location = new Guid(Locationid);
            }
            var searchResult = this._searchService.Search(searchQuery);


            return searchResult;
        }
    }
}
