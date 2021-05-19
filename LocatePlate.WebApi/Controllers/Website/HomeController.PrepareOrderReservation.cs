using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.RestaurantDomain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace LocatePlate.WebApi.Controllers.Website
{
    public partial class HomeController
    {
        [HttpGet("getaddtoorder/{menuId}/{url}/{cityname}")]
        public IActionResult GetAddToOrder(int menuId, string url, string cityname)
        {
            var menu = this._menuService.Get(menuId);
            MenuReservation model = new MenuReservation();
            if (menu != null)
            {

                model.Id = menu.Id;
                model.RestaurantId = menu.RestaurantId;
                model.Name = menu.Name;
                model.Price = menu.Price;
                model.Url = url;
                model.CityName = cityname;
            }
            return View("_AddToOrder", model);
        }

        [HttpPost("addtoorder/{date}")]
        public IActionResult AddToOrder(MenuReservation menuReservationModel, DateTime date)
        {
            List<MenuReservation> reservations = null;
            var order = "";
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{menuReservationModel.RestaurantId}", out order);

            var promocode = GetPromoCode(menuReservationModel.RestaurantId);
            if (!string.IsNullOrEmpty(order))
                reservations = JsonSerializer.Deserialize<List<MenuReservation>>(order);
            if (reservations == null)
                reservations = new List<MenuReservation>();

            reservations.Add(menuReservationModel);
            string jsonString = JsonSerializer.Serialize(reservations);
            this._contextAccessor.HttpContext.Response.Cookies.Append($"order{menuReservationModel.RestaurantId}", jsonString);

            var model = new Reservation { StartDate = date, GrandTotal = 0.0, Discount = 0.0, SubTotal = 0.0, PromoCode = promocode, MenuItems = reservations };
            if (reservations.Any()) CalculatePrice(ref model);

            return PartialView("_Order", model);
        }

        [HttpGet("applypromo/{promocode}/{restaurantId}/{date}")]
        public IActionResult ApplyPromo(string promocode, int restaurantId, DateTime date)
        {
            List<MenuReservation> reservations = null;
            var order = "";
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{restaurantId}", out order);
            this._contextAccessor.HttpContext.Response.Cookies.Append($"promo{restaurantId}", promocode);

            if (!string.IsNullOrEmpty(order))
                reservations = JsonSerializer.Deserialize<List<MenuReservation>>(order);
            var model = new Reservation { StartDate = date, GrandTotal = 0.0, Discount = 0.0, SubTotal = 0.0, PromoCode = promocode, MenuItems = reservations };
            if (reservations.Any()) CalculatePrice(ref model);

            return PartialView("_Order", model);
        }

        [HttpGet("removepromo/{restaurantId}/{date}")]
        public IActionResult RemovePromo(int restaurantId, DateTime date)
        {
            List<MenuReservation> reservations = null;
            var order = "";
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{restaurantId}", out order);
            this._contextAccessor.HttpContext.Response.Cookies.Append($"promo{restaurantId}", "");

            if (!string.IsNullOrEmpty(order))
                reservations = JsonSerializer.Deserialize<List<MenuReservation>>(order);
            var model = new Reservation { StartDate = date, GrandTotal = 0.0, Discount = 0.0, SubTotal = 0.0, PromoCode = "", MenuItems = reservations };
            if (reservations.Any()) CalculatePrice(ref model);

            return PartialView("_Order", model);
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
                    //  booking.BookingXMenu.Add(new BookingXMenu { MenuId = menu.Id, SpecialInstruction = menuItems.SpecialInstruction, RestaurantId = menuItems.RestaurantId });
                }

                string locationId = "";
                getLocationIdFromCookie(ref locationId);
                var location = new Guid(locationId);
                var page = this._pageService.Get(location);

                reservation.SubTotal = totalPrice;
                reservation.Tax = Math.Round(reservation.SubTotal * ((double)page.ProvinceTax / 100), 2, MidpointRounding.ToEven);
                reservation.GrandTotal = reservation.SubTotal + reservation.Tax;
                reservation.HavingPromoCode = !string.IsNullOrEmpty(reservation.PromoCode);
                // check for any discount'
                if (reservation.HavingPromoCode) CalculateGrandTotalPrice(ref reservation);
            }
        }

        [HttpGet("removeorder/{menuId}/{restaurantId}/{quantity}/{date}")]
        public IActionResult removeorder(int menuId, int restaurantId, int quantity, DateTime date)
        {
            var menuReservationModels = getMenuReservationModelFromCookie(restaurantId);
            if (menuReservationModels != null)
            {
                if (quantity == 0)
                    menuReservationModels.RemoveAll(c => c.Id == menuId);
                else
                    foreach (var item in menuReservationModels)
                    {
                        if (item.Id == menuId)
                            item.Quantity = quantity;
                    }
                string jsonString = JsonSerializer.Serialize(menuReservationModels);
                this._contextAccessor.HttpContext.Response.Cookies.Append($"order{restaurantId}", jsonString);
            }
            var promocode = GetPromoCode(restaurantId);
            var model = new Reservation { StartDate = date, GrandTotal = 0.0, Discount = 0.0, SubTotal = 0.0, PromoCode = promocode, MenuItems = menuReservationModels };
            if (menuReservationModels.Any()) CalculatePrice(ref model);

            return View("_Order", model);
        }

        [HttpGet("addquantity/{menuId}/{restaurantId}/{quantity}/{date}")]
        public IActionResult AddQuantity(int menuId, int restaurantId, int quantity, DateTime date)
        {
            var menuReservationModels = getMenuReservationModelFromCookie(restaurantId);

            if (menuReservationModels != null)
            {
                foreach (var item in menuReservationModels)
                {
                    if (item.Id == menuId)
                        item.Quantity = quantity;
                }
                string jsonString = JsonSerializer.Serialize(menuReservationModels);
                this._contextAccessor.HttpContext.Response.Cookies.Append($"order{restaurantId}", jsonString);
            }

            var promocode = GetPromoCode(restaurantId);

            var model = new Reservation { StartDate = date, GrandTotal = 0.0, Discount = 0.0, SubTotal = 0.0, PromoCode = promocode, MenuItems = menuReservationModels };
            if (menuReservationModels.Any()) CalculatePrice(ref model);

            return View("_Order", model);
        }

        [NonAction]
        public void GetOrder(ref Restaurant restaurant)
        {
            var reservations = GetOrder(restaurant.Id);
            if (reservations == null)
                reservations = new List<MenuReservation>();
            foreach (var menu in restaurant.Menus)
            {
                if (reservations.Any(a => a.Id == menu.Id))
                {
                    menu.IsAddedToCart = true;
                }
            }
            var model = new Reservation { MenuItems = reservations };
            if (reservations.Any()) CalculatePrice(ref model);
            restaurant.ReservationModel = model;
        }

        private List<MenuReservation> GetOrder(int Id)
        {
            List<MenuReservation> reservations = null;
            var order = "";
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{Id}", out order);
            if (!string.IsNullOrEmpty(order))
                reservations = JsonSerializer.Deserialize<List<MenuReservation>>(order);

            return reservations;
        }

        [NonAction]
        public List<RatingGroups> GetReview(int restaurantId) => this._reviewService.GetAllByRestaurantId(restaurantId);


        private bool IsPromoCodeValid(ref Reservation booking, ref Discount promo) => booking.StartDate.Date >= promo.ValidFrom && booking.StartDate.Date <= promo.ValidTo;

        private Discount GetDiscount(ref Reservation booking) => this._discountService.GetDiscountByCodeAndRestaurantId(booking.PromoCode, booking.MenuItems.FirstOrDefault().RestaurantId);

        private List<MenuReservation> getMenuReservationModelFromCookie(int restaurantId)
        {

            List<MenuReservation> menuReservationModels = null;
            var order = "";
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"order{restaurantId}", out order);
            if (!string.IsNullOrEmpty(order))
                menuReservationModels = JsonSerializer.Deserialize<List<MenuReservation>>(order);
            return menuReservationModels;
        }

        private string GetPromoCode(int restaurantId)
        {
            var promoCode = "";
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue($"promo{restaurantId}", out promoCode);

            return promoCode ?? "";
        }
    }
}
