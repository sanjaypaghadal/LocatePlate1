using LocatePlate.Infrastructure.Domain;
using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Bookings;
using LocatePlate.Service.Capactities;
using LocatePlate.Service.Menus;
using LocatePlate.Service.Pages;
using LocatePlate.Service.Restaurants;
using LocatePlate.Service.Services.Discounts;
using LocatePlate.WebApi.Contracts.V1;
using LocatePlate.WebApi.Controllers.V1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class PrepareOrderController : BaseController
    {
        private readonly IMenuService _menuService;
        private readonly IPageService _pageService;
        private readonly IDiscountService _discountService;

        public PrepareOrderController(IMenuService menuService, IPageService pageService, IDiscountService discountService, IClientSide clientSide, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {

            _menuService = menuService;
            _pageService = pageService;
            _discountService = discountService;
        }

        [HttpPost("addtoorder")]
        public IActionResult AddToOrder(Booking booking)
        {
            var model = new Reservation { StartDate = booking.Date, LocationId = booking.LocationId, GrandTotal = 0.0, Discount = 0.0, SubTotal = 0.0, PromoCode = booking.PromoCode, MenuItems = booking.MenuItems };
            if (model?.MenuItems != null)
                CalculatePrice(ref model);

            return Ok(model);
        }

        private void CalculateGrandTotalPrice(ref Reservation reservation)
        {
            var discount = GetDiscount(ref reservation);
            if (discount?.MinimumPrice != null && discount.MinimumPrice <= reservation.SubTotal)
            {
                reservation.IsValidPromoCode = IsPromoCodeValid(ref reservation, ref discount);
                var discountedPrice = discount.Price;
                if (reservation.IsValidPromoCode)
                {
                    if (discount.Price > 0)
                        reservation.GrandTotal = reservation.GrandTotal = discountedPrice;

                    if (discount.Percent > 0)
                        reservation.GrandTotal = reservation.GrandTotal.ComputeDiscountedPrice(discount.Percent, out discountedPrice);
                }
                reservation.Discount = discountedPrice;
            }
            else if (discount?.MinimumPrice != null)
            {
                reservation.IsMinimumPriceNotMeet = true;
                reservation.MinimumPrice = discount.MinimumPrice;
            }
        }

        private void CalculatePrice(ref Reservation reservation)
        {
            if (reservation.MenuItems.Any())
            {
                double totalPrice = 0.0;
                foreach (var menuItems in reservation.MenuItems)
                {
                    var menu = this._menuService.Get(menuItems.Id);
                    totalPrice += (menu.Price * menuItems.Quantity);
                }

                var page = this._pageService.Get(reservation.LocationId);

                reservation.SubTotal = totalPrice;
                reservation.Tax = Math.Round(reservation.SubTotal * ((double)page.ProvinceTax / 100), 2, MidpointRounding.ToEven);
                reservation.GrandTotal = reservation.SubTotal + reservation.Tax;
                reservation.HavingPromoCode = !string.IsNullOrEmpty(reservation.PromoCode);
                // check for any discount'
                if (reservation.HavingPromoCode) CalculateGrandTotalPrice(ref reservation);
            }
        }

        private bool IsPromoCodeValid(ref Reservation booking, ref Discount promo) => booking.StartDate.Date >= promo.ValidFrom && booking.StartDate.Date <= promo.ValidTo;

        private Discount GetDiscount(ref Reservation booking) => this._discountService.GetDiscountByCodeAndRestaurantId(booking.PromoCode, booking.MenuItems.FirstOrDefault().RestaurantId);
    }
}
