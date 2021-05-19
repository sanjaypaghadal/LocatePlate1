using LocatePlate.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Restaurant : BaseEntity
    {
        [Required]
        [Display(Name = "Name *")]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }
        [MaxLength(5000)]
        [Column(TypeName = "VARCHAR")]
        public string About { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR")]
        public string Tags { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR")]
        public string Cuisine { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string CountryName { get; set; }
        [Required]
        [Display(Name = "Country *")]
        public int Country { get; set; } = 38;
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string ProvinceName { get; set; }
        [Required]
        [Display(Name = "Province *")]
        public int Province { get; set; } = 9;
        [Required]
        [Display(Name = "City *")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid LocationId { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string CityName { get; set; }
        [Required]
        [JsonIgnore]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Locality *")]
        public string Locality { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Full Address *")]
        public string FullAddress { get; set; }
        [JsonIgnore]
        public string ExtraFields { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [JsonIgnore]
        [Display(Name = "Cover Image")]
        [MaxLength(300)]
        [Column(TypeName = "VARCHAR")]
        public string CoverImages { get; set; } = string.Empty;
        [JsonIgnore]
        [MaxLength(400)]
        [Column(TypeName = "VARCHAR")]
        public string Specialities { get; set; }
        [MaxLength(400)]
        [Column(TypeName = "VARCHAR")]
        public string Recommendations { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string Url { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "PinCode *")]
        [Column(TypeName = "VARCHAR")]
        public string PinCode { get; set; }
        [Display(Name = "CostForTwo")]
        public double CostForTwo { get; set; }
        [JsonIgnore]
        public virtual string RestaurantUrl
        {
            get
            {
                return $"{Name?.Replace(' ', '-').Replace('#', '-').Replace('$', '-').Replace('%', '-').Replace('^', '-').Replace('*', '-').Replace('@', '-').Replace('!', '-').Replace('&', '-')}-{Locality?.Replace(' ', '-').Replace(' ', '-').Replace('#', '-').Replace('$', '-').Replace('%', '-').Replace('^', '-').Replace('*', '-').Replace('@', '-').Replace('!', '-').Replace('&', '-')}";
            }
        }
        public ResturantType ResturantType { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string ResturantTypeName { get; set; }
        [NotMapped]
        public double? RatingCount { get; set; }
        [NotMapped]
        public double? RatingTotalCount { get; set; }
        [NotMapped]
        public List<RatingGroups> RatingGroups { get; set; }
        [NotMapped]
        public List<Ratings> Ratings { get; set; }
        [NotMapped]
        public List<RatingGroups> ReviewRating { get; set; }
        [NotMapped]
        public int? ReviewCount { get; set; }
        [NotMapped]
        public virtual Reservation ReservationModel { get; set; }

        public virtual string MobileCoverImage
        {
            get
            {
                if (string.IsNullOrEmpty(CoverImages))
                    return "../../images/website/no-image.png";
                return $"/UploadImages/{UserId}/{Id}/{CoverImages.Split(',')[0]}";
            }
        }

        public virtual List<string> MobileCoverImages
        {
            get
            {
                List<string> images = new List<string>();
                if (!string.IsNullOrEmpty(CoverImages))
                    foreach (var image in CoverImages.Split(','))
                    {
                        images.Add($"/UploadImages/{UserId}/{Id}/{image}");
                    }
                return images;
            }
        }

        public ICollection<Menu> Menus { get; set; }
        public ICollection<Timing> Timings { get; set; }
        //public ICollection<Capacity> Catacity { get; set; }

        [JsonIgnore]
        public ICollection<Discount> Discounts { get; set; }
        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; }

        [NotMapped]
        public ICollection<PartyKeyValueModel> PartyList { get; set; }
    }

    public enum ResturantType
    {
        [Display(Name = "Select Resturant Type")] NotSelected,
        [Display(Name = "Fast Food/Drive-thrus")] FastFoods,
        [Display(Name = "Fast Casual")] FastCasual,
        [Display(Name = "Sports Bar")] SportsBar,
        [Display(Name = "Casual Dining")] CasualDining,
        [Display(Name = "Fine Dining")] FineDining,
        [Display(Name = "Pop-up Restaurants")] PopupRestaurants,
        [Display(Name = "Food Trucks")] FoodTrucks,
        [Display(Name = "Hotel Dining")] HotelDining,
        [Display(Name = "Bar/Pub")] BarPub,
        [Display(Name = "Sweet Shop")] SweetShop,
        [Display(Name = "Cafe/Bakery")] CafeBakery,
        [Display(Name = "Takeaway")] Takeaway,
        [Display(Name = "Other")] Other
    }

    public class RatingGroups
    {
        public string Key { get; set; }
        public List<Ratings> Value { get; set; }
        public double AvgPercentage { get; set; }
        public double Avg { get; set; }
        public Reviews Reviews { get; set; }
    }

    public class PartyKeyValueModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}