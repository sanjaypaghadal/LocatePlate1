using LocatePlate.Model.RestaurantDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.WebApi.Controllers.Website
{
    public partial class HomeController
    {
        [HttpGet("availability/{restaurantId}/{partysize}/{date}")]
        public IActionResult availability(int restaurantId, int partysize, DateTime date)
        {
            var localSlots = new System.Collections.Generic.List<string>();
            if (restaurantId != 0)
            {
                var dateTimeUnspec = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
                var utcdate = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, this._ClientSide.ClientInfo);
                if (utcdate.Date != date.Date) utcdate = date;
                var slots = this._bookingService.GetAvailableSlot(restaurantId, partysize, utcdate, null);
                if (slots.Any()) //TODO
                    slots.ToList().ForEach(c => localSlots.Add(TimeZoneInfo.ConvertTimeFromUtc(c, this._ClientSide.ClientInfo).ToString("hh:mm tt")));
            }
            return Ok(GetAvailableSlotSelectListItem(localSlots));
        }

        [HttpGet("SetLocation/{locationid}/{locationame}")]
        public IActionResult SetLocation(Guid locationid, string locationame)
        {
            setlocationInCookie(locationid, locationame);
            return Ok();
        }

        [NonAction]
        public void GetBookingInfo(int Id)
        {
            var partyList = GetPartySizeSelectListItem(Id);
            var defaultParty = partyList.Count > 1 ? Convert.ToInt32(partyList.Skip(1).Take(1).FirstOrDefault()?.Value) : 0;

            //var dateTimeUnspec = DateTime.SpecifyKind(_ClientSide.ClientTime, DateTimeKind.Unspecified);
            //var utcdate = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, this._ClientSide.ClientInfo);
            //if (utcdate.Date != _ClientSide.ClientTime.Date) utcdate = _ClientSide.ClientTime;

            var slots = this._bookingService.GetAvailableSlot(Id, defaultParty, _ClientSide.ClientTime, null);
            //if (slots.Any()) //TODO
            //{
            //    slots.ForEach(c => TimeZoneInfo.ConvertTimeFromUtc(c, _ClientSide.ClientInfo).ToString("hh:mm tt"));
            //}
            var Slots = GetAvailableSlotSelectListItem(slots);
            if (Slots.Count() == 0)
            {
                Slots.Add(new SelectListItem() { Text = "Close", Value = "Close" });
            }
            ViewBag.Slots = Slots;

            ViewBag.Party = partyList;
        }

        private List<SelectListItem> GetPartySizeSelectListItem(int restuarantId)
        {
            var partyList = new List<SelectListItem> { new SelectListItem { Text = "No. Of Guests", Value = "0" } };
            this._capacityService.GetAllByRestaurantId(restuarantId).ForEach(c => partyList.Add(new SelectListItem { Text = $"{c.Size} Or Less", Value = $"{c.Size}" }));
            return partyList;
        }

        private List<SelectListItem> GetAvailableSlotSelectListItem(List<DateTime> slots)
        {
            var slotDropDown = new List<SelectListItem>();
            slotDropDown.Add(new SelectListItem { Text = "Select Time", Value = "0" });
            slots.ToList().ForEach(c => slotDropDown.Add(new SelectListItem { Text = $"{c.ToString("hh:mm tt")}", Value = $"{c}" }));

            return slotDropDown;
        }

        private List<SelectListItem> GetAvailableSlotSelectListItem(List<string> slots)
        {
            var slotDropDown = new List<SelectListItem>();
            slotDropDown.Add(new SelectListItem { Text = "Select", Value = "0" });
            slots.ToList().ForEach(c => slotDropDown.Add(new SelectListItem { Text = c, Value = c }));

            return slotDropDown;
        }

        private List<Capacity> GetPartySize(int restuarantId) => this._capacityService.GetAllByRestaurantId(restuarantId).ToList();


    }
}
