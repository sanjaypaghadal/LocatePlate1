using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Services.Discounts;
using LocatePlate.WebApi.Contracts.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Admin
{

    [Route(AdminRoutes.BaseRoute.Admin)]
    public class DiscountController : BaseController
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _discountService = discountService;
        }

        //get
        [HttpGet("")]
        [HttpGet("{pageindex?}/{pagesize?}")]
        public IActionResult Index(int pageindex = 0, int pagesize = 10)
        {
            ViewBag.LocationName = LocationName;
            ViewBag.RestaurantName = RestaurantName;

            var discounts = this._discountService.GetAllByUserAndRestaurant(UserId, RestaurantId, pageindex, pagesize);
            var customDeals = discounts.Where(c => c.IsCustom = true).Select(c => c.PromoCode).ToList();
            var siteDeals = this._discountService.GetByUrl(LocationName, customDeals);
            ViewBag.SiteDeals = siteDeals;
            return View(discounts);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(int Id)
        {
            if (Id > 0)
            {
                ViewData["Title"] = "Edit";
                var discount = await this._discountService.GetAsync(Id);
                return View(discount);
            }
            return View(new Discount());
        }

        [HttpGet("AddSiteDeal/{Id}")]
        public IActionResult AddSiteDeal(int Id)
        {
            var siteDeal = this._discountService.Get(Id);
            if (siteDeal != null)
            {
                var now = DateTime.UtcNow;
                var model = new Discount
                {
                    UserId = UserId,
                    ModifiedDate = now,
                    ModifiedBy = User.Identity.Name,
                    PromoCode = siteDeal.PromoCode.Trim(),
                    RestaurantId = RestaurantId,
                    CreatedDate = now,
                    CreatedBy = User.Identity.Name,
                    LocationUrl = LocationName,
                    DealUrl = siteDeal.DealUrl,
                    IsCustom = true,
                    Percent = siteDeal.Percent,
                    Price = siteDeal.Price,
                    MinimumPrice=siteDeal.MinimumPrice,
                    IsSoftDelete = false,
                    TermAndCondition=siteDeal.TermAndCondition,
                    ValidFrom=siteDeal.ValidFrom,
                    ValidTo=siteDeal.ValidTo
                };
                this._discountService.Insert(model);
            }
            return RedirectToAction(nameof(Index));
        }

        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Discount model)
        {
            ModelState.Remove("Id"); // This will remove the key 
            if (ModelState.IsValid)
            {
                //var siteDeals = (List<Discount>)ViewBag.SiteDeals;
                ////deal url  should not clash
                //if (siteDeals.Any(c => c.DealUrl == model.DealUrl && c.Name == model.Name))
                //{
                //    ModelState.AddModelError("clash", "Name or Deal url is clashing with one of the site deals");
                //    return View(model);
                //}

                model.UserId = UserId;
                model.ModifiedDate = DateTime.UtcNow;
                model.ModifiedBy = User.Identity.Name;
                model.PromoCode = model.PromoCode.Trim();
                model.RestaurantId = RestaurantId;
                model.IsCustom = true;
                if (model.Id == 0)
                {
                    model.CreatedDate = model.ModifiedDate;
                    model.CreatedBy = model.ModifiedBy;
                    this._discountService.Insert(model);
                }
                else
                    this._discountService.Update(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id > 0)
            {
                var discount = await this._discountService.GetAsync(Id);
                this._discountService.Delete(discount);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
