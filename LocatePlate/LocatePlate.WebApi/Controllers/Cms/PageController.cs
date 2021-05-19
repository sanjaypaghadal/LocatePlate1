using LocatePlate.Model.Cms;
using LocatePlate.Service.Pages;
using LocatePlate.Service.PagesLayouts;
using LocatePlate.Service.Restaurants;
using LocatePlate.Service.Sections;
using LocatePlate.Service.Services.Discounts;
using LocatePlate.Service.Services.Modules;
using LocatePlate.WebApi.Contracts.CmsRoutes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Cms
{
    [Route(CmsRoute.BaseRoute.Cms)]
    public partial class PageController : BaseController
    {
        private readonly IPageService _pageService;
        private readonly IPageLayoutService _pageLayoutService;
        private readonly IModuleService _moduleService;
        private readonly ISectionService _sectionService;
        private readonly IRestaurantService _restaurantService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHostingEnvironment _environment;
        private readonly IDiscountService _discountService;

        public PageController(IPageService pageService, IPageLayoutService pageLayoutService, IModuleService moduleService, ISectionService sectionService, IRestaurantService restaurantService, IHostingEnvironment environment, IDiscountService discountService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this._pageService = pageService;
            this._pageLayoutService = pageLayoutService;
            this._moduleService = moduleService;
            this._sectionService = sectionService;
            this._restaurantService = restaurantService;
            this._contextAccessor = httpContextAccessor;
            this._environment = environment;
            this._discountService = discountService;
        }

        [HttpGet("", Name ="cmshomepage")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public async Task<IActionResult> Index(int pageindex = 0, int pagesize = 10)
        {
            var pages = await this._pageService.GetAllAsync(pageindex, pagesize);
            return View(pages);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(Guid Id)
        {
            ViewData["Title"] = "Create";
            if (Id != Guid.Empty)
            {
                ViewData["Title"] = "Edit";
                var page = await this._pageService.GetAsync(Id);
                ViewBag.PageLayout = GetSectionsSelectedListItem(page?.PageLayout?.Id);

                return View(page);
            }
            ViewBag.PageLayout = GetSectionsSelectedListItem(null);

            return View(new Page());
        }

        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            ModelState.Remove("Id"); // This will remove the key 
                page.UserId = UserId;
                page.ModifiedDate = DateTime.UtcNow;
                page.ModifiedBy = User.Identity.Name;
                page.Title = $"{page.Title}";
                page.Url = page.Url.ToLower();
                // CarouseImages(page);
                if (page?.PageLayout?.Section != null)
                {
                    foreach (var sect in page.PageLayout.Section.Select((value, i) => new { i, value }))
                    {
                        var item = sect?.value;
                        //if (item?.Module?.CardsVerticalModule != null)
                        //    item.Module.CardsVerticalModule.Markup = GenerateRestaurantMarkup(item?.Module?.CardsVerticalModule, page.Url);
                        //if (item?.Module?.SearchButtonsModule?.Buttons != null)
                        //    item.Module.SearchButtonsModule.Markup = GenerateSearchButtonModuleMarkup(item?.Module?.SearchButtonsModule, page.Url);
                        if (item?.Module?.DealModule?.Deals != null)
                            await SaveDeals(item?.Module?.DealModule, page.Url, sect.i);
                    }
                }
                if (page.Id == Guid.Empty)
                {
                    page.CreatedDate = page.ModifiedDate;
                    page.CreatedBy = page.ModifiedBy;
                    this._pageService.Save(page);
                }
                else
                    this._pageService.Update(page);

            return RedirectToRoute("cmshomepage");
        }


        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var restaurant = await this._pageService.GetAsync(Id);
                this._pageService.Delete(restaurant);
                return RedirectToRoute("cmshomepage");
            }
            return View();
        }


    }
}
