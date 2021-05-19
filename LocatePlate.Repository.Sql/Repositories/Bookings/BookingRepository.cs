using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Repository.Bookings
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly LocatePlateContext _locatePlateContext;

        public BookingRepository(LocatePlateContext locatePlateContext) : base(locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public List<Booking> GetAllBookingsByRestaurant(int RestaurantId, DateTime startTime, DateTime endTime)//, DateTime? from, DateTime? to)
        {
            return this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId && c.StartTime == startTime).ToList();
        }

        public List<Booking> GetBookingsByRestaurantPartySizeAndStartTime(int RestaurantId, int partySize, DateTime startTime) =>
             // already booked time slots for same party size 
             this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId && c.Date.Date == startTime.Date && c.PartySize == partySize && c.IsAccept == true).ToList();

        public bool IsAvailableTimeSlots(int RestaurantId, int partySize, DateTime startTime)
        {
            // already booked time slots for same party size 
            var booking = this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId && c.Date.Date == startTime.Date && c.PartySize == partySize && c.IsAccept == true).ToList();//;.Select(c=>c.StartTime).ToList();

            //  capacity avaialble for the partysize
            var availableSeatingPlan = this._locatePlateContext.Capacities.Where(c => c.RestaurantId == RestaurantId && c.Size == partySize).ToList();

            return (availableSeatingPlan.Count() - booking.Count()) >= 1 ? true : false;
        }

        public override Booking Insert(Booking entity)
        {
            var booking = base.Insert(entity);
            return booking;
        }
        public override IEnumerable<Booking> GetAllByUser(Guid userId)
        {
            return this._locatePlateContext.Bookings.Include(c => c.Restaurant).Include(c => c.MenuItems).Where(c => c.UserId == userId).AsEnumerable();
        }

        public List<Booking> GetAllPendingBookingsByRestaurant(int RestaurantId)
        {
            //return this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId && c.IsCancelled == false && c.IsAccept == false).Include(c => c.Restaurant).Include(c => c.MenuItems).ToList();
            return this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId && c.IsCancelled == false).Include(c => c.Restaurant).Include(c => c.MenuItems).ToList();
        }

        public int GetAllPendingBookingsByRestaurantCount(int RestaurantId)
        {
            return this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId && c.IsCancelled == false && c.IsAccept == false).Include(c => c.Restaurant).Include(c => c.MenuItems).Count();
        }
        public PaginatedList<Booking> GetAllBookingsByRestaurant(int RestaurantId, int page, int pageSize)
        {
            return new PaginatedList<Booking>(this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId).OrderByDescending(c=>c.Date.Date).Include(c => c.Restaurant).Include(c => c.MenuItems).AsQueryable(), page, pageSize);
        }

        public bool AcceptRejectOrder(Booking entity)
        {
            bool result = false;
            var model = this._locatePlateContext.Bookings.FirstOrDefault(c => /*c.Date.Date >= DateTime.UtcNow && c.StartTime.TimeOfDay >= DateTime.UtcNow.TimeOfDay &&*/ c.Id == entity.Id);
            if (model != null)
            {
                model.IsAccept = entity.IsAccept;
                model.IsCancelled = entity.IsCancelled;
                this._locatePlateContext.Bookings.Update(model);
                this._locatePlateContext.SaveChanges();
                result = true;
            }
            return result;
        }

    }
}
