using LocatePlate.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Booking : BaseEntity
    {
        public DateTime Date { get; set; }        
        public DateTime StartTime { get; set; }
        
        //add by binal patel start
        [NotMapped]
        public string StartTimeDisplay { get; set; }
        //add by binal patel start
        public int PartySize { get; set; }
        [JsonIgnore]
        public bool IsCancelled { get; set; }
        [JsonIgnore]
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string CancelReason { get; set; }
        public bool IsAccept { get; set; }
        public bool IsFoodOrder { get; set; }
        public double TotalTax { get; set; }
        public double TotalPrice { get; set; }
        public override int RestaurantId { get; set; }
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public string PromoCode { get; set; } = string.Empty;
        public bool HavingPromoCode { get; set; }
        public bool IsValidPromoCode { get; set; }
        public string Token { get; set; }
        //[JsonIgnore]
        //[NotMapped]
        public string BillId { get; set; }
        [JsonIgnore]
        public Restaurant Restaurant { get; set; }
        public List<MenuReservation> MenuItems { get; set; }
        [NotMapped]
        public bool ReservationType { get; set; } //  true for take away and false for dine in

        [JsonIgnore]
        [NotMapped]
        public Reservation ReservationModel { get; set; }

        [JsonIgnore]
        [NotMapped]
        public ICollection<BookingXMenu> BookingXMenu { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual TimeZoneInfo ClientTime { get; set; }
        //public ICollection<BookingXCapacity> BookingXCapacity { get; set; }
        [NotMapped]
        public Guid LocationId { get; set; }

        //add by binal start
        public bool IsCheckOut { get; set; }
        public int CapicityId { get; set; }
        //end
    }
}
