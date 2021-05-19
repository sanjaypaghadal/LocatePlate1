using LocatePlate.Repository.Context;
using LocatePlate.Service.Bookings;
using LocatePlate.WebApi.Contracts.Mvc;
using LocatePlate.WebApi.Controllers.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.Mvc
{
    //[Authorize]
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class DashboardController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly LocatePlateContext _context;

        public DashboardController(IBookingService bookingService, LocatePlateContext context, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _bookingService = bookingService;
            _context = context;
        }
        [HttpGet("{DeviceToken?}")]
        public IActionResult Index(string DeviceToken)
        {
            try
            {
                var user = _context.UserIdentities.FirstOrDefault(c => c.Id == $"{UserId}");
                Int32 restaurantId= _context.Restaurants.FirstOrDefault(r =>  r.UserId.ToString() == user.Id).Id;
                var pendingBookings = this._bookingService.GetAllPendingBookingsByRestaurant(RestaurantId);                
               
                if (user != null && !string.IsNullOrEmpty(DeviceToken))
                {
                    user.DeviceId = DeviceToken;
                    user.IsAndroid = true;
                    _context.SaveChanges();
                }
                return View(pendingBookings);
            }
            catch (Exception)
            {
                return View();
            }
            
        }

        

        [HttpGet("GetBookingData/{StartDate}/{EndDate}")]
        public IActionResult GetBookings(DateTime StartDate, DateTime EndDate)
        {
            var result = this._bookingService.GetAllBookingsByRestaurant(RestaurantId, StartDate, EndDate);
            return PartialView("_BookingDashboard", result);
        }
    }
}
