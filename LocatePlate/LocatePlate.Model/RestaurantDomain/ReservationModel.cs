using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public List<MenuReservation> MenuItems { get; set; }
        public DateTime StartDate { get; set; }
        [NotMapped]
        public DateTime Date { get; set; }
        public int Party { get; set; }
        public double GrandTotal { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public string PromoCode { get; set; } = string.Empty;
        public bool HavingPromoCode { get; set; }
        public bool IsValidPromoCode { get; set; }
        [NotMapped]
        public double MinimumPrice { get; set; }
        [NotMapped]
        public bool IsMinimumPriceNotMeet { get; set; }

        [NotMapped]
        public bool ReservationType { get; set; } //  true for take away and false for dine in
        [NotMapped]
        public string RestaurantName { get; set; }

        [NotMapped]
        public Guid LocationId { get; set; }
    }

    public class MenuReservation
    {
        [Key]
        public int Id { get; set; }
        public int MenuId { get; set; }
        [JsonIgnore]
        public string SpecialInstruction { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int BookingId { get; set; }
    }

}
