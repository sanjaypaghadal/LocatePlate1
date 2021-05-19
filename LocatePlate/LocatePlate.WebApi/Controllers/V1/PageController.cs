using LocatePlate.Service.Pages;
using LocatePlate.Service.Restaurants;
using LocatePlate.WebApi.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IRestaurantService _restaurantService;
        public PageController(IPageService pageService, IRestaurantService restaurantService)
        {
            this._pageService = pageService;
            this._restaurantService = restaurantService;
        }

        [HttpGet("GetAll")]
        public IActionResult Index()
        {
            var page = this._pageService.GetAllAsync().GetAwaiter().GetResult().OrderBy(c => c.Name).Select(c => new { Id = c.Id, Name = c.Url }).ToList();
            return Ok(page);
        }

        [HttpGet("{Id}")]
        public IActionResult Index(Guid Id)
        {
            var page = this._pageService.GetFullPageById(Id);

            return Ok(page);
        }
    }
}
