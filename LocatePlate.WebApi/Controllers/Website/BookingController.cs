using LocatePlate.Infrastructure.Constant;
using LocatePlate.Infrastructure.Domain;
using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Bookings;
using LocatePlate.Service.Menus;
using LocatePlate.Service.Services.Discounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using static LocatePlate.Infrastructure.Extentions.doubleHelpers;

namespace LocatePlate.WebApi.Controllers.Website
{
    [Route("booking")]
    //[Authorize(Policy = UserRoles.WebsiteUser, AuthenticationSchemes = UserRoles.WebsiteUser)]
    [Authorize]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IClientSide _clientSide;
        private readonly IDiscountService _discountService;
        private readonly IMenuService _menuService;
        private readonly IHttpContextAccessor _contextAccessor;

        public BookingController(IBookingService bookingService, IMenuService menuService, IClientSide clientSide, IDiscountService discountService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._bookingService = bookingService;
            this._clientSide = clientSide;
            this._discountService = discountService;
            this._contextAccessor = contextAccessor;
            this._menuService = menuService;
        }

        [HttpPost("reservation")]
        public IActionResult Reservation(int restaurantId)
        {
            try
            {
                string json = "";
                this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"booking{restaurantId}", out json);
                Booking Booking = JsonSerializer.Deserialize<Booking>(json);

                Booking.UserId = UserId;
                Booking.ModifiedDate = DateTime.UtcNow;
                Booking.ModifiedBy = User.Identity.Name;
                var dateTimeUnspec = DateTime.SpecifyKind(Booking.StartTime, DateTimeKind.Unspecified);
                Booking.StartTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, this._clientSide.ClientInfo);

                if (Booking?.MenuItems != null)
                    foreach (var item in Booking?.MenuItems)
                        item.RestaurantId = Booking.RestaurantId;
                //var discount = GetDiscount(ref booking);
                //var isValidOffer = IsPromoCodeValid(ref booking ,ref discount);
                //if (isValidOffer)
                //{
                //    List<MenuReservationModel> resrvations = null;
                //    var order = "";
                //    this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{booking.RestaurantId}", out order);
                //    if (!string.IsNullOrEmpty(order))
                //        resrvations = JsonSerializer.Deserialize<List<MenuReservationModel>>(order);
                //    booking.BookingXMenu = new List<BookingXMenu>();
                //    double totalPrice = 0.0;
                //    foreach (var menuItems in resrvations)
                //    {
                //        var menu = this._menuService.Get(menuItems.Id);
                //        totalPrice += (menu.Price * menuItems.Quantity);
                //        booking.BookingXMenu.Add(new BookingXMenu { MenuId = menu.Id, SpecialInstruction = menuItems.SpecialInstruction, RestaurantId = menuItems.RestaurantId });
                //    }

                //    /// apply discount
                //    if (discount.Price > 0)
                //        totalPrice = totalPrice = discount.Price;

                //    if (discount.Percent > 0)
                //        totalPrice= totalPrice.ComputeDiscountedPrice(discount.Percent);
                //}

                var bookingResult = this._bookingService.Update(Booking);
                return RedirectToAction("open", "payment", Booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("checkout")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("confirmation/{bookingid}")]
        public IActionResult Confirmation(int bookingid)
        {
            var booking = this._bookingService.Get(bookingid);
            booking.IsAccept = true;
            this._bookingService.Update(booking);
            return View();
        }

        //[HttpPost("reservation")]
        //public IActionResult CheckIfIsPromoCodeValid(Booking booking) {

        //    var discount = GetDiscount(ref booking);
        //    var isValidOffer = IsPromoCodeValid(ref booking, ref discount);
        //    if (isValidOffer)
        //    {
        //        List<MenuReservationModel> resrvations = null;
        //        var order = "";
        //        this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{booking.RestaurantId}", out order);
        //        if (!string.IsNullOrEmpty(order))
        //            resrvations = JsonSerializer.Deserialize<List<MenuReservationModel>>(order);
        //        booking.BookingXMenu = new List<BookingXMenu>();
        //        double totalPrice = 0.0;
        //        foreach (var menuItems in resrvations)
        //        {
        //            var menu = this._menuService.Get(menuItems.Id);
        //            totalPrice += (menu.Price * menuItems.Quantity);
        //            booking.BookingXMenu.Add(new BookingXMenu { MenuId = menu.Id, SpecialInstruction = menuItems.SpecialInstruction, RestaurantId = menuItems.RestaurantId });
        //        }

        //        /// apply discount
        //        if (discount.Price > 0)
        //            totalPrice = totalPrice = discount.Price;

        //        if (discount.Percent > 0)
        //            totalPrice = totalPrice.ComputeDiscountedPrice(discount.Percent);
        //    }

        //    return View("");
        //}



        private bool IsPromoCodeValid(ref Booking booking, ref Discount promo) => booking.StartTime.Date >= promo.ValidFrom && booking.StartTime.Date <= promo.ValidTo;

        private Discount GetDiscount(ref Booking booking) => this._discountService.GetDiscountByCodeAndRestaurantId(booking.PromoCode, booking.RestaurantId);
    }
}
