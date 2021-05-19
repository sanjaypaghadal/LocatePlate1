using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Bookings;
using LocatePlate.Service.Capactities;
using LocatePlate.Service.Restaurants;
using LocatePlate.WebApi.Contracts.V1;
using LocatePlate.WebApi.Controllers.V1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ICapacityService _capacityService;
        private readonly IBookingService _bookingService;

        public RestaurantController(IRestaurantService restaurantService, ICapacityService capacityService, IBookingService bookingService)
        {
            _restaurantService = restaurantService;
            _capacityService = capacityService;
            _bookingService = bookingService;
        }

        [HttpGet("{url}/{restauranturl}")]
        public IActionResult RestaurantDetail(string url, string restauranturl)
        {
            var restaurant = this._restaurantService.GetDetailByUrl(restauranturl, url);

            restaurant.CityName = url;
            restaurant.PartyList = GetPartySizeSelectListItem(restaurant.Id);
            return Ok(restaurant);
        }

        [HttpGet("getall")]
        public IActionResult RestaurantDetail()
        {
            var restaurants = this._restaurantService.GetAll();
            return Ok(restaurants);
        }

        [HttpGet("getbookinginfo/{id}/{clientDateTime}")]
        public IActionResult GetBookingInfo(int id, DateTime clientDateTime)
        {
            var booking = new BookingInfo();
            booking.PartyCapacity = this._capacityService.GetAllByRestaurantId(id);
            var defaultParty = booking.PartyCapacity.Count > 1 ? Convert.ToInt32(booking.PartyCapacity.Skip(1).Take(1).FirstOrDefault()?.Size) : 0;
            booking.Slots = this._bookingService.GetAvailableSlot(id, defaultParty, clientDateTime, null);

            return Ok(booking);
        }

        private List<PartyKeyValueModel> GetPartySizeSelectListItem(int restuarantId)
        {
            List<PartyKeyValueModel> party = new List<PartyKeyValueModel>();
            this._capacityService.GetAllByRestaurantId(restuarantId).ForEach(c => party.Add(new PartyKeyValueModel { Key = $"{c.Size} Or Less", Value = $"{c.Size}" }));
            return party;
        }
    }
}