using LocatePlate.Infrastructure.Constant;
using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.Search;
using LocatePlate.Service.Pages;
using LocatePlate.Service.Services.SearchManager;
using LocatePlate.WebApi.Contracts.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;
        private readonly IPageService _pageService;
        private readonly IClientSide _clientSide;
        private readonly IHttpContextAccessor _contextAccessor;


        public SearchController(ISearchService searchService, IPageService pageService, IHttpContextAccessor contextAccessor, IClientSide clientSide) : base(contextAccessor)
        {
            this._searchService = searchService;
            this._pageService = pageService;
            this._clientSide = clientSide;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("search")]
        public IActionResult Search(SearchQuery searchQuery)
        {
            try
            {
                searchQuery.ClientInfo = clientInfoZ;
                searchQuery.ClientTime = clientTime;
                var result = this._searchService.Search(searchQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("autodetectlocation/{logitude}/{latitude}")]
        public IActionResult autodetectlocation(decimal logitude, decimal latitude)
        {
            try
            {
                var searchQuery = new SearchQuery { Latitude = logitude, Logitude = latitude };
                var currentLocation = this._searchService.GetNearByLocation(searchQuery);
                return Ok(currentLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "no location found!");
            }
        }

        [HttpGet("getfiltermetadata")]
        public IActionResult FillFilter()
        {
            var filtermeta = this._searchService.GetFilterData();
            return Ok(filtermeta);
        }


    }
}