using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;

namespace LocatePlate.Repository.Bookings
{
    public class BookingXCapacityRepository : BaseRepository<BookingXCapacity>, IBookingXCapacityRepository
    {
        public BookingXCapacityRepository(LocatePlateContext locatePlateContext) : base(locatePlateContext)
        {
        }
    }
}
