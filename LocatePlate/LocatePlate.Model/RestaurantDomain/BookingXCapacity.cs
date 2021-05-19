using LocatePlate.Infrastructure.Domain;

namespace LocatePlate.Model.RestaurantDomain
{
    public class BookingXCapacity : BaseEntity
    {
        public int BookingId { get; set; }
        public int CapacityId { get; set; }
        //public Booking Booking { get; set; }
        //public Capacity Capacity { get; set; }
    }
}
