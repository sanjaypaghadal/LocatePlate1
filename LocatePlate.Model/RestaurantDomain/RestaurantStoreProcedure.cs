using System;
using System.Text.Json.Serialization;

namespace LocatePlate.Model.RestaurantDomain
{
    public record RestaurantStoreProcedure
    {
        public Guid Id { get; init; }
        public string MenuItem { get; init; }
        public double Price { get; init; }
        public string Calories { get; init; }
        public string FoodNatureName { get; init; }
        public string Images { get; init; }
        public string MenuCategoryName { get; init; }
        public string MenuAbout { get; init; }
        public string About { get; init; }
        public string RestaurantName { get; init; }
        public double CostForTwo { get; init; }
        public string FullAddress { get; init; }
        public string CoverImages { get; init; }
        public string Cuisine { get; init; }
        public int RestaurantId { get; init; }
        public Guid UserId { get; init; }
        public double? RatingCount { get; init; }
        public int? ReviewCount { get; init; }
        public int MenuId { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
        public Guid LocationId { get; set; }
    }
}
