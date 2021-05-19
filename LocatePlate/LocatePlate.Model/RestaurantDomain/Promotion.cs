using LocatePlate.Infrastructure.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Promotion : BaseEntity
    {
        [MaxLength(20)]
        [Column(TypeName = "VARCHAR")]
        public string PromoCode { get; set; }
        [MaxLength(2000)]
        [Column(TypeName = "VARCHAR")]
        public string TermAndCondition { get; set; }
        public PromoType PromoType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Double? Price { get; set; }
        public Double? Percentage { get; set; }
        public override int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public enum PromoType
    {
        Site, Restaurant
    }
}
