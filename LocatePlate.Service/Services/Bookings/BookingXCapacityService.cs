using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Bookings;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.Bookings
{
    public class BookingXCapacityService : BaseService<BookingXCapacity, IBookingXCapacityRepository>, IBookingXCapacityService
    {
        private readonly IBookingXCapacityRepository _bookingXCapacityRepository;

        public BookingXCapacityService(IBookingXCapacityRepository bookingXCapacityRepository) : base(bookingXCapacityRepository)
        {
        }
    }
}
