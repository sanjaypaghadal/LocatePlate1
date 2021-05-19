using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;

namespace LocatePlate.Repository.Bookings
{
    public class BookingXMenuRepository : BaseRepository<BookingXMenu>, IBookingXMenuRepository
    {
        public BookingXMenuRepository(LocatePlateContext locatePlateContext) : base(locatePlateContext)
        {
        }
    }
}
