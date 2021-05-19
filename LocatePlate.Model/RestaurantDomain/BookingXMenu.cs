using LocatePlate.Infrastructure.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class BookingXMenu : BaseEntity
    {
        public int BookingId { get; set; }
        public int MenuId { get; set; }
        [MaxLength(1000)]
        [Column(TypeName = "VARCHAR")]
        public string SpecialInstruction { get; set; }
    }
}
