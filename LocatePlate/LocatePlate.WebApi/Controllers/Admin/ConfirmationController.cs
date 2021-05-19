using LocatePlate.Service.Bookings;
using LocatePlate.WebApi.Contracts.Mvc;
using LocatePlate.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocatePlate.WebApi.Controllers.Admin
{
    [Route(AdminRoutes.BaseRoute.Admin)]
    public class ConfirmationController : BaseController
    {

        private readonly IBookingService _bookingService;
        public ConfirmationController(IBookingService bookingService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this._bookingService = bookingService;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {

            var pendingBookings = this._bookingService.GetAllPendingBookingsByRestaurant(RestaurantId);

            return View(pendingBookings);
        }


        [HttpGet("AllOrders/{pageindex?}/{pagesize?}")]
        public IActionResult AllOrders(int pageindex = 0, int pagesize = 10)
        {
            var pendingBookings = this._bookingService.GetAllBookingsByRestaurant(RestaurantId, pageindex, pagesize);
            return View(pendingBookings);
        }

        [HttpPost("AcceptOrder")]
        public IActionResult AcceptOrder(int bookingId)
        {
            try
            {
                var result = this._bookingService.AcceptRejectOrder(new Model.RestaurantDomain.Booking { Id = bookingId, IsAccept = true, IsCancelled = false });
            }
            catch (System.Exception)
            {

            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost("RejectOrder")]
        public IActionResult RejectOrder(int bookingId)
        {
            try
            {
                var result = this._bookingService.AcceptRejectOrder(new Model.RestaurantDomain.Booking { Id = bookingId, IsAccept = false, IsCancelled = true });
            }
            catch (System.Exception)
            {

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("CheckoutOrder")]
        public IActionResult CheckoutOrder(int bookingId)
        {
            try
            {
                var result = this._bookingService.AcceptRejectOrder(new Model.RestaurantDomain.Booking { Id = bookingId, IsAccept = true, IsCancelled = false ,IsCheckOut=true});
            }
            catch (System.Exception)
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private void PushNotification()
        {
            FCMHelper.SendPushNotification("LocatePlate", "New order", "there is new order", UserId, RestaurantId, 1);

        }
    }
}
