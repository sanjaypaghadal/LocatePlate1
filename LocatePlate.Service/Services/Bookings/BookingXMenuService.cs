using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Bookings;
using LocatePlate.Service.Abstract;

namespace LocatePlate.Service.Bookings
{
    public class BookingXMenuService : BaseService<BookingXMenu, IBookingXMenuRepository>, IBookingXMenuService
    {
        private readonly IBookingXMenuRepository _bookingRepository;

        public BookingXMenuService(IBookingXMenuRepository bookingRepository) : base(bookingRepository)
        {
        }
    }
}
