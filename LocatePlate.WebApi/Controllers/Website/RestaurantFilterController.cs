 using LocatePlate.Service.Services.SearchManager;
using Microsoft.AspNetCore.Mvc;

namespace LocatePlate.WebApi.Controllers.Website
{
    public class RestaurantFilterController : Controller
    {
        private readonly ISearchService _searchService;
        public RestaurantFilterController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        public IActionResult Index()
        {
            var filtermeta = this._searchService.GetFilterData();
            return View();
        }
    }
}
