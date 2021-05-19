using LocatePlate.Infrastructure.Constant;
using LocatePlate.Service.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocatePlate.WebApi.Controllers.Website
{
    [Route("RestaurantOrderHistory")]
    [Authorize(UserRoles.WebsiteUser)]
    public class RestaurantOrderHistoryController : BaseController
    {
        private readonly IBookingService _bookingService;

        public RestaurantOrderHistoryController(IBookingService bookingService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._bookingService = bookingService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var bookings = this._bookingService.GetAllByUser(UserId);
            return View(bookings);
        }
    }
}
