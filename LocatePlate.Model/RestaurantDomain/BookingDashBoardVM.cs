using System.Collections.Generic;
using System.Linq;

namespace LocatePlate.Model.RestaurantDomain
{
    public class BookingDashBoardVM
    {
        public int TotalAllOrder
        {
            get
            {
                return TotalOrders.Count();
            }
        }
        public int TotalReservationWithoutFood
        {
            get
            {
                return TotalOrders.Where(c => c.IsFoodOrder == false && c.IsAccept == true).Count();
            }
        }
        public int TotalReservationWithFood
        {
            get
            {
                return TotalOrders.Where(c => c.IsFoodOrder == true && c.IsAccept == true).Count();
            }
        }
        //public int TotalTakeAway
        //{
        //    get
        //    {
        //        return TotalOrders.Where(c => c.IsFoodOrder == true && c.IsAccept == true).Count();
        //    }
        //}
        public int TotalCancelled
        {
            get
            {
                return TotalOrders.Where(c => c.IsCancelled == true && c.IsAccept == true).Count();
            }
        }

        public int TotalReturiningUsers
        {
            get
            {
                return TotalOrders.GroupBy(x => x.UserId).Where(g => g.Count() > 1).Count();
            }
        }

        public double TotalEarning
        {
            get
            {
                return TotalOrders.Sum(c => c.TotalPrice);
            }
        }

        public double TotalTax
        {
            get
            {
                return TotalOrders.Sum(c => c.TotalTax);
            }
        }
        public List<Booking> TotalOrders { get; set; }


    }
}
