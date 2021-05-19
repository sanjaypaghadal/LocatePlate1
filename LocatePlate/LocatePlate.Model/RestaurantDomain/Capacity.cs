using LocatePlate.Infrastructure.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Capacity : BaseEntity
    {
        [Required]
        [DisplayName("Table Name")]
        [MaxLength(20)]
        [Column(TypeName = "VARCHAR")]
        public string TableName { get; set; }
        [DisplayName("Size")]
        [Required]
        public int Size { get; set; }
        [DisplayName("Area")]
        [Required]
        public Area Area { get; set; }
        public override int RestaurantId { get; set; }

        [DisplayName("Restaurant")]
        public Restaurant Restaurant { get; set; }

        //public ICollection<BookingXCapacity> BookingXCapacity { get; set; }
    }

    public enum Area
    {
        Indoor,
        Outdoor
    }
}
