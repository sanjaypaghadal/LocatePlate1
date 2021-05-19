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
            return new PaginatedList<Booking>(this._locatePlateContext.Bookings.Where(c => c.RestaurantId == RestaurantId).OrderByDescending(c => c.Date.Date).Include(c => c.Restaurant).Include(c => c.MenuItems).AsQueryable(), page, pageSize);
        }

        public bool AcceptRejectOrder(Booking entity)
        {
            bool result = false;
            var model = this._locatePlateContext.Bookings.FirstOrDefault(c => /*c.Date.Date >= DateTime.UtcNow && c.StartTime.TimeOfDay >= DateTime.UtcNow.TimeOfDay &&*/ c.Id == entity.Id);
            if (model != null)
            {
                model.IsAccept = entity.IsAccept;
                model.IsCancelled = entity.IsCancelled;
                model.IsCheckOut = entity.IsCheckOut;
                this._locatePlateContext.Bookings.Update(model);
                this._locatePlateContext.SaveChanges();
                result = true;
            }
            return result;
        }

        //add by binal
        public Booking InsertBooking(Booking entity)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var findRestaurantName = _context.Restaurants.FirstOrDefault(r => Convert.ToInt32(r.Id) == Convert.ToInt32(entity.RestaurantId));
                    var splitRestaurantName = findRestaurantName.Name.Split();
                    var createRestaurantCode = "";
                    long maxOrderNo = 0;

                    if (splitRestaurantName.Length >= 2)
                    {
                        createRestaurantCode = splitRestaurantName[0].Substring(0, 1) + splitRestaurantName[1].Substring(0, 1);
                    }
                    else
                    {
                        createRestaurantCode = splitRestaurantName[0].Substring(0, 1);
                    }
                    var createOrderNo = createRestaurantCode + DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd");

                    try
                    {
                        maxOrderNo = _context.BookingOrderNumber
                                       .Where(c => c.RestaurantId == entity.RestaurantId)
                                       .Max(c => c.OrderNo);
                    }
                    catch (Exception exe)
                    {
                        maxOrderNo = 0;
                    }

                    //call for placing zero to create orderno
                    createOrderNo = PlaceZeroBeforeNumber(maxOrderNo, createOrderNo);


                    if (entity == null) throw new ArgumentNullException("entity");

                    var allTables = from c in _context.Capacities
                             where c.RestaurantId == entity.RestaurantId && c.Size == entity.PartySize
                             select c.Id;

                    var bookedCapicityId = from ci in _context.Bookings
                             where ci.RestaurantId == entity.RestaurantId && ci.PartySize == entity.PartySize
                             && ci.Date==entity.Date && ci.StartTime==entity.StartTime && ci.IsCheckOut==false
                             select ci.CapicityId;


                    var checkoutCapacityId = allTables.Except(bookedCapicityId);

                    if (checkoutCapacityId.Count() > 0)
                    {
                        entity.BillId = createOrderNo;
                        entity.IsCheckOut = false;
                        entity.CapicityId = checkoutCapacityId.FirstOrDefault();
                        _context.Bookings.Add(entity);
                        _context.SaveChanges();

                        BookingOrderNumber bookingOrderNumber = new BookingOrderNumber();
                        bookingOrderNumber.OrderNo = maxOrderNo + 1; //> 0 ? maxOrderNo + 001 : 001;
                        bookingOrderNumber.BillId = createOrderNo;
                        bookingOrderNumber.BookingId = _context.Bookings.FirstOrDefault(r => r.BillId == createOrderNo).Id;
                        bookingOrderNumber.RestaurantId = entity.RestaurantId;


                        _context.BookingOrderNumber.Add(bookingOrderNumber);
                        _context.SaveChanges();
                        transaction.Commit();

                    }
                    else
                    {
                        throw new ArgumentNullException("entity");
                    }
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ArgumentNullException("entity");
                }
            }
        }
        //end
        
        private string PlaceZeroBeforeNumber(long number, string generateOrderNo)
        {
            if (number <= 9)
            {
                generateOrderNo = generateOrderNo + "000" + (number + 1);
            }
            else if (number <= 99)
            {
                generateOrderNo = generateOrderNo + "00" + (number + 1);
            }
            else if (number <= 999)
            {
                generateOrderNo = generateOrderNo + "0" + (number + 1);
            }
            else
            {
                generateOrderNo = generateOrderNo + (number + 1);
            }
            return generateOrderNo;
        }
        public System.Data.DataTable GetPerformanceIndicator(int RestaurantId)
        {
            throw new ArgumentNullException("entity");
        }
    }
}
