using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;

namespace LocatePlate.Repository.Repositories.Search
{
    public interface ISearchManager
    {
        SearchRecords Search(SearchQuery searchQuery);
        SearchRecords SearchDeals(SearchQuery searchQuery);
        SearchRecords SearchTags(SearchQuery searchQuery);
        FilterCriteria GetFilterData();
        CurrentLocation GetNearByLocation(SearchQuery searchQuery);
    }
}
