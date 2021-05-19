using LocatePlate.Infrastructure.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Reviews : BaseEntity
    {
        [MaxLength(2000)]
        [Column(TypeName = "VARCHAR")]
        public string Review { get; set; }
    }

    public class ReviewRating
    {
        public Reviews Reviews { get; set; }
        public Ratings Ratings { get; set; }
    }
}
