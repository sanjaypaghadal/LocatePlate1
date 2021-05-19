using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;
using LocatePlate.Repository.Repositories.Search;
using LocatePlate.Service.Bookings;
using System;
using System.Linq;

namespace LocatePlate.Service.Services.SearchManager
{
    public class SearchService : ISearchService
    {
        private readonly ISearchManager _searchManager;
        private readonly IBookingService _bookingService;

        public SearchService(ISearchManager searchManager, IBookingService bookingService)
        {
            this._searchManager = searchManager;
            this._bookingService = bookingService;
        }

        public SearchRecords Search(SearchQuery searchQuery)
        {
            var result = this._searchManager.Search(searchQuery);
            FillSlots(searchQuery, ref result);
            return result;
        }
        public SearchRecords SearchDeals(SearchQuery searchQuery)
        {
            var result = this._searchManager.SearchDeals(searchQuery);
            FillSlots(searchQuery, ref result);
            return result;
        }
        public SearchRecords SearchTags(SearchQuery searchQuery)
        {
            var result = this._searchManager.SearchTags(searchQuery);
            FillSlots(searchQuery, ref result);
            return result;
        }
        public FilterCriteria GetFilterData() => this._searchManager.GetFilterData();
        public CurrentLocation GetNearByLocation(SearchQuery searchQuery) => this._searchManager.GetNearByLocation(searchQuery);

        private void FillSlots(SearchQuery searchQuery, ref SearchRecords results)
        {
            results.CityName = results.SearchResults.FirstOrDefault()?.CityName;
            foreach (var item in results.SearchResults)
            {
                var slots = this._bookingService.GetAvailableSlot(item.RestaurantId, searchQuery.PartySize, searchQuery.SearchDate, null);
                item.Slots = new System.Collections.Generic.List<string>();
                FillImages(item);
                if (slots.Any()) //TODO
                {
                    slots.ToList().ForEach(c => item.Slots.Add(TimeZoneInfo.ConvertTimeFromUtc(c, searchQuery.ClientInfo).ToString("hh:mm tt")));
                }
            }
        }

        private void FillImages(SearchResult item)
        {
            if (!string.IsNullOrEmpty(item.Images))
                item.Images = item.Images.Split(',').Length > 0 ? $"/UploadImages/{item.UserId}/{item.RestaurantId}/{item.Images.Split(',')[0]}" : $"/UploadImages/{item.UserId}/{item.Name}/{item.Images}";
        }
    }
}
