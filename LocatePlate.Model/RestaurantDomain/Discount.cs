using LocatePlate.Infrastructure.Domain;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Discount : BaseEntity
    {
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string PromoCode { get; set; }
        public double Percent { get; set; }
        [DisplayName( "Minimum price to apply discount")]
        public double MinimumPrice { get; set; }
        public double Price { get; set; }
        public bool IsCustom { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string DealUrl { get; set; } = string.Empty;
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string LocationUrl { get; set; }
        [MaxLength(5000)]
        [Column(TypeName = "VARCHAR")]
        public string TermAndCondition { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
        public DateTime ValidTo { get; set; } = DateTime.UtcNow;
    }
}
