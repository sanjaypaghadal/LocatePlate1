using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Bookings;
using LocatePlate.Repository.Capactities;
using LocatePlate.Repository.Timings;
using LocatePlate.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Service.Bookings
{
    public class BookingService : BaseService<Booking, IBookingRepository>, IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITimingRepository _timingRepository;
        private readonly ICapacityRepository _capacityRepository;
        private readonly IClientSide _ClientSide;
        public BookingService(IBookingRepository bookingRepository, ITimingRepository timingRepository, ICapacityRepository capacityRepository, IClientSide clientSide) : base(bookingRepository)
        {
            _bookingRepository = bookingRepository;
            _timingRepository = timingRepository;
            _capacityRepository = capacityRepository;
            this._ClientSide = clientSide;
        }

        public BookingDashBoardVM GetAllBookingsByRestaurant(int RestaurantId, DateTime startTime, DateTime endTime)
        {
            return new BookingDashBoardVM { TotalOrders = this._bookingRepository.GetAllBookingsByRestaurant(RestaurantId, startTime, endTime) };
        }
        public List<Booking> GetBookingsByRestaurantPartySizeAndStartTime(int RestaurantId, int partySize, DateTime startTime) => _bookingRepository.GetBookingsByRestaurantPartySizeAndStartTime(RestaurantId, partySize, startTime);
        public bool IsAvailableTimeSlots(int RestaurantId, int partySize, DateTime startTime) => _bookingRepository.IsAvailableTimeSlots(RestaurantId, partySize, startTime);
        public List<DateTime> GetAvailableSlot(int restaurantId, int partysize, DateTime date, Area? area)
        {
            var slots = new List<DateTime>();
            //possible avialable time for the day
            var timing = this._timingRepository.GetCurrentDayTime(restaurantId, date);
            if (timing == null) return slots;
            //slots = GetPossibleSlot(date.Date == DateTime.UtcNow.Date ? RoundUp(date) : timing.StartTime, timing.CloseTime);

            //var dateTimeUnspec = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            //var currentdate = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, this._ClientSide.ClientInfo);
            timing.StartTime = new DateTime(date.Year, date.Month, date.Day, timing.StartTime.Hour, timing.StartTime.Minute, 0);
            timing.CloseTime = new DateTime(date.Year, date.Month, date.Day, timing.CloseTime.Hour, timing.CloseTime.Minute, 0);
            slots = GetPossibleSlot(timing.StartTime, timing.CloseTime);

            //get all bookings of the day for restuarant with partysize
            var bookings = _bookingRepository.GetBookingsByRestaurantPartySizeAndStartTime(restaurantId, partysize, date);

            // get the seating arrangement possible by partysize in the restuarant
            var allTables = area == null ?
                _capacityRepository.GetCapacityByRestaurantAndPartySize(restaurantId, partysize)
                : _capacityRepository.GetCapacityByRestaurantPartySizeAndArea(restaurantId, partysize, area.GetValueOrDefault());

            if (bookings.Any())
                for (int i = 0; i < slots.Count; i++)
                {
                    var totalTableBooked = bookings.Where(c => c.StartTime == slots[i]).Count();
                    if ((allTables.Count() - totalTableBooked) < 1)
                    {
                        slots.RemoveAt(i);
                        i--;
                    }
                }

            return slots;
        }

        public List<Booking> GetAllPendingBookingsByRestaurant(int RestaurantId) => _bookingRepository.GetAllPendingBookingsByRestaurant(RestaurantId);
        public PaginatedList<Booking> GetAllBookingsByRestaurant(int RestaurantId, int page, int pageSize) => _bookingRepository.GetAllBookingsByRestaurant(RestaurantId, page, pageSize);
        public bool AcceptRejectOrder(Booking entity) => _bookingRepository.AcceptRejectOrder(entity);
        public int GetAllPendingBookingsByRestaurantCount(int RestaurantId) => _bookingRepository.GetAllPendingBookingsByRestaurantCount(RestaurantId);

        //private List<DateTime> GetPossibleSlot(DateTime date, DateTime endTime)
        //{
        //    var TimeSlots = new List<DateTime>();
        //    //var minutes = endTime.Subtract(new DateTime(endTime.Year, endTime.Month,
        //    //     endTime.Day, date.Hour, date.Minute, 0)).TotalMinutes;

        //    //if (date.Date == DateTime.UtcNow.Date)
        //    //{
        //    //    var diff = DateTime.UtcNow.Subtract(date).TotalMinutes;
        //    //    if (diff <= 0)
        //    //        date = DateTime.UtcNow.AddMinutes(45);
        //    //    else
        //    //        date = Block45Min(date);
        //    //}
        //    date = RoundUp(date);
        //    var starttime = new DateTime(endTime.Year, endTime.Month, endTime.Day, date.Hour, date.Minute, 0);
        //    if (starttime != endTime)
        //    {
        //        var minutes = endTime.Subtract(starttime).TotalMinutes;
        //        var slots = minutes / 15;
        //        // as we block first 45 min
        //        if (minutes > 0)
        //        {
        //            for (int i = 0; i < slots; i++)
        //            {
        //                //if (i == 0 && date.Date == DateTime.UtcNow.Date)
        //                //    date = date.AddMinutes(45);
        //                TimeSlots.Add(date);
        //                date = date.AddMinutes(15);
        //            }
        //        }
        //        else if (minutes < 0)
        //        {
        //            for (int i = 0; i < Math.Abs(slots); i++)
        //            {
        //                //if (i == 0 && date.Date == DateTime.UtcNow.Date)
        //                //    date = date.AddMinutes(45);
        //                TimeSlots.Add(date);
        //                date = date.AddMinutes(15);
        //            }
        //        }
        //    }
        //    else if (starttime == endTime)
        //    {
        //        endTime = endTime.AddDays(1).AddHours(-12);
        //        var minutes = endTime.Subtract(starttime).TotalMinutes;
        //        var slots = minutes / 15;
        //        slots = slots - 3;
        //        for (int i = 0; i < slots; i++)
        //        {
        //            //if (i == 0 && date.Date == DateTime.UtcNow.Date)
        //            //    date = date.AddMinutes(45);
        //            TimeSlots.Add(date);
        //            date = date.AddMinutes(15);
        //        }
        //    }
        //    return TimeSlots;
        //}

        private List<DateTime> GetPossibleSlot(DateTime date, DateTime endTime)
        {
            var TimeSlots = new List<DateTime>();
            if (this._ClientSide.ClientTime.Date != date.Date)
            {
                if (date != endTime)
                {
                    var minutes = endTime.Subtract(new DateTime(endTime.Year, endTime.Month, endTime.Day, date.Hour, date.Minute, 0)).TotalMinutes;
                    var slots = minutes / 15;
                    if (minutes > 0)
                        for (int i = 0; i < slots; i++)
                        {
                            TimeSlots.Add(date);
                            date = date.AddMinutes(15);
                        }
                    else
                        for (int i = 0; i < Math.Abs(slots); i++)
                        {
                            TimeSlots.Add(date);
                            date = date.AddMinutes(15);
                        }
                }
                else if (date == endTime)
                {
                    endTime = endTime.AddDays(1).AddHours(-12);
                    var minutes = endTime.Subtract(date).TotalMinutes;
                    var slots = minutes / 15;
                    for (int i = 0; i < slots; i++)
                    {
                        TimeSlots.Add(date);
                        date = date.AddMinutes(15);
                    }
                }
            }
            else if (this._ClientSide.ClientTime.Date == date.Date)
            {
                if (date == endTime)
                {
                    date = new DateTime(endTime.Year, endTime.Month, endTime.Day, this._ClientSide.ClientTime.TimeOfDay.Hours, this._ClientSide.ClientTime.TimeOfDay.Minutes, 0);
                    date = RoundUp(date).AddMinutes(45);
                    endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 11, 59, 59);
                    var minutes = endTime.Subtract(new DateTime(endTime.Year, endTime.Month, endTime.Day, date.TimeOfDay.Hours, date.TimeOfDay.Minutes, 0)).TotalMinutes;
                    var slots = minutes / 15;
                    for (int i = 0; i < slots; i++)
                    {
                        TimeSlots.Add(date);
                        date = date.AddMinutes(15);
                    }
                }
                else
                {
                    var tminutes = this._ClientSide.ClientTime.TimeOfDay.Subtract(date.TimeOfDay).TotalMinutes;
                    if (tminutes < 0)
                        date = date.AddMinutes(Math.Abs(tminutes) + 45);
                    else if (tminutes > 0)
                    {
                        if (tminutes < 45)
                            tminutes = tminutes + (45 - tminutes);
                        date = date.AddMinutes(tminutes);
                    }
                    date = RoundUp(date);
                    var minutes = endTime.Subtract(new DateTime(endTime.Year, endTime.Month, endTime.Day, date.Hour, date.Minute, 0)).TotalMinutes;
                    var slots = minutes / 15;
                    for (int i = 0; i < slots; i++)
                    {
                        TimeSlots.Add(date);
                        date = date.AddMinutes(15);
                    }
                }
            }
            return TimeSlots;
        }

        private DateTime RoundUp(DateTime dateTime)
        {
            var Hour = dateTime.Hour;
            var min = (dateTime.Minute / 15) * 15;
            if (min != 45) min += 15; else { min = 0; Hour = Hour + 1; }
            return new DateTime(dateTime.Year, dateTime.Month,
                 dateTime.Day, Hour, min, 0);
        }
    }
}