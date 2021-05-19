using LocatePlate.Infrastructure.Constant;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Service.Bookings;
using LocatePlate.Service.Capactities;
using LocatePlate.WebApi.Contracts.V1;
using LocatePlate.WebApi.Controllers.V1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class ReservationController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly ICapacityService _capacityService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReservationController(IBookingService bookingService, ICapacityService capacityService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._bookingService = bookingService;
            this._capacityService = capacityService;
            this._contextAccessor = contextAccessor;
        }

        [HttpPost("booking")]
        public IActionResult Reservation(Booking booking)
        {
            try
            {
                booking.ModifiedDate = DateTime.UtcNow;
                booking.ModifiedBy = User.Identity.Name;
                var dateTimeUnspec = DateTime.SpecifyKind(booking.StartTime, DateTimeKind.Unspecified);
                booking.StartTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, booking.ClientTime);
                var bookingResult = this._bookingService.Insert(booking);
                return Ok(true);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("getbookinginfo")]
        public IActionResult GetBookingInfo(int Id)
        {
            var partyList = GetPartySizeSelectListItem(Id);
            var defaultParty = partyList.Count > 1 ? Convert.ToInt32(partyList.Take(1).FirstOrDefault().Value) : 0;

            var clientTi = clientTime;

            var slots = this._bookingService.GetAvailableSlot(Id, defaultParty, clientTi, null);
            var SlotsList = GetAvailableSlotSelectListItem(slots);

            return Ok(new { Slots = SlotsList, Party = partyList });
        }

        [HttpGet("availability/{restaurantId}/{partysize}/{date}")]
        public IActionResult availability(int restaurantId, int partysize, DateTime date)
        {
            var clientInfo = clientInfoZ;
            var localSlots = new List<KeyValueModel>();
            if (restaurantId != 0)
            {
                var dateTimeUnspec = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
                date = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, clientInfo);
                var slots = this._bookingService.GetAvailableSlot(restaurantId, partysize, date, null);
                if (slots.Any()) //TODO
                    slots.ToList().ForEach(c => localSlots.Add(new KeyValueModel { Key = TimeZoneInfo.ConvertTimeFromUtc(c, clientInfo).ToString("hh:mm tt"), Value = $"{TimeZoneInfo.ConvertTimeFromUtc(c, clientInfo)}" }));
            }
            return Ok(localSlots);
        }

        private List<KeyValueModel> GetPartySizeSelectListItem(int restuarantId)
        {
            List<KeyValueModel> party = new List<KeyValueModel>();
            this._capacityService.GetAllByRestaurantId(restuarantId).ForEach(c => party.Add(new KeyValueModel { Key = $"{c.Size} Guests", Value = $"{c.Size}" }));
            return party;
        }

        private List<KeyValueModel> GetAvailableSlotSelectListItem(List<DateTime> slots)
        {
            var clientInfo = clientInfoZ;

            List<KeyValueModel> slot = new List<KeyValueModel>();

            slots.ToList().ForEach(c => slot.Add(new KeyValueModel { Key = $"{TimeZoneInfo.ConvertTimeFromUtc(c, clientInfo).ToString("hh:mm tt")}", Value = $"{TimeZoneInfo.ConvertTimeFromUtc(c, clientInfo)}" }));
            return slot;
        }

        [HttpGet("OrderHistoryByUserId")]
        public IActionResult Index(Guid UserId)
        {
            var bookings = this._bookingService.GetAllByUser(UserId);
            return Ok(bookings);
        }
    }
}