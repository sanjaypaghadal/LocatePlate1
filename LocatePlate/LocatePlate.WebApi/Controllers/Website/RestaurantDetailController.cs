using LocatePlate.Service.Bookings;
using LocatePlate.Service.Restaurants;
using LocatePlate.Service.Timings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocatePlate.WebApi.Controllers.Website
{
    [Route("restaurant")]
    public class RestaurantDetailController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IBookingService _bookingService;
        private readonly ITimingService _timingService;

        public RestaurantDetailController(IRestaurantService restaurantService, IBookingService bookingService, IHttpContextAccessor contextAccessor) 
            //: base(contextAccessor)
        {
            this._restaurantService = restaurantService;
            this._bookingService = bookingService;
        }

        [HttpGet("{Id}")]
        public IActionResult Index(int Id)
        {
            var restuarant = this._restaurantService.Get(Id);
            return View(restuarant);
        }


    }
}
