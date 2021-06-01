using LocatePlate.Model.Cms.Modules.Abstract;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LocatePlate.Model.Cms.Modules.Restaurants
{
    [BsonIgnoreExtraElements]
    public class CardsListingModule : BaseModule, IModuleEntity
    {
        public static readonly Guid _id = Guid.Parse("8be9fb68-0745-4d51-8565-f38a980903f3");
        public Guid Id { get => _id; }
        public List<RestaurantDictonary> Restaurants { get; init; } = new List<RestaurantDictonary>();
        [BsonIgnore]
        public virtual List<Restaurant> RestaurantDetails { get; set; }

    }

    public record RestaurantDictonary { public int Id { get; init; } public string Name { get; init; } }
}
