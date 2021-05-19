using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;

namespace LocatePlate.Service.Services.SearchManager
{
    public interface ISearchService
    {
        SearchRecords Search(SearchQuery searchQuery);
        SearchRecords SearchDeals(SearchQuery searchQuery);
        SearchRecords SearchTags(SearchQuery searchQuery);
        FilterCriteria GetFilterData();
        CurrentLocation GetNearByLocation(SearchQuery searchQuery);
    }
}
