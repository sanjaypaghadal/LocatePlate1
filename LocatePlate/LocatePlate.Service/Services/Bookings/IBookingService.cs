using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;
using System.Data;

namespace LocatePlate.Service.Bookings
{
    public interface IBookingService : IBaseService<Booking>
    {
        BookingDashBoardVM GetAllBookingsByRestaurant(int RestaurantId, DateTime startTime, DateTime endTime);
        List<Booking> GetBookingsByRestaurantPartySizeAndStartTime(int RestaurantId, int partySize, DateTime startTime);
        bool IsAvailableTimeSlots(int RestaurantId, int partySize, DateTime startTime);
        List<DateTime> GetAvailableSlot(int restaurantId, int partysize, DateTime date, Area? area);
        List<Booking> GetAllPendingBookingsByRestaurant(int RestaurantId);
        PaginatedList<Booking> GetAllBookingsByRestaurant(int RestaurantId, int page, int pageSize);
        bool AcceptRejectOrder(Booking entity);
        int GetAllPendingBookingsByRestaurantCount(int RestaurantId);

        //add by binal start
        Booking InsertBooking(Booking entity);

        DataTable GetPerformanceIndicator(int RestaurantId);
        //end
    }
}
