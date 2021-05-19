using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Capactities;
using LocatePlate.Service.Restaurants;
using LocatePlate.WebApi.Contracts.Mvc;
using LocatePlate.WebApi.Controllers.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Mvc
{
    //[Authorize]
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class CapacityController : BaseController
    {
        private readonly ICapacityService _capacityService;
        private readonly IRestaurantService _restaurantService;
        public CapacityController(ICapacityService capacityService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._capacityService = capacityService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var capacities = this._capacityService.GetAllByUserAndRestaurant(UserId, RestaurantId);
            return View(capacities);
        }

        [HttpGet("Create")]
        [HttpGet("Edit/{Id}")]
        public async Task<IActionResult> Create(int Id)
        {
            ViewData["Title"] = "Create";
            if (Id > 0)
            {
                ViewData["Title"] = "Edit";
                var restaurant = await this._capacityService.GetAsync(Id);
                return View(restaurant);
            }
            return View();
        }
        //post
        [HttpPost("Create")]
        [HttpPost("Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Insert(Capacity capacity)
        {
            ModelState.Remove("Id"); // This will remove the key 
            if (ModelState.IsValid)
            {
                capacity.UserId = UserId;
                capacity.ModifiedDate = DateTime.UtcNow;
                capacity.ModifiedBy = User.Identity.Name;
                capacity.RestaurantId = RestaurantId;
                if (capacity.Id == 0)
                {
                    capacity.CreatedDate = capacity.ModifiedDate;
                    capacity.CreatedBy = capacity.ModifiedBy;
                    this._capacityService.Insert(capacity);
                }
                else
                    this._capacityService.Update(capacity);
                return RedirectToAction(nameof(Index));
            }
            return View(capacity);
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id > 0)
            {
                var restaurant = await this._capacityService.GetAsync(Id);
                this._capacityService.Delete(restaurant);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
