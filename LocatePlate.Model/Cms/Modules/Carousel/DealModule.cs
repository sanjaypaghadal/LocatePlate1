using LocatePlate.Model.Cms.Modules.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LocatePlate.Model.Cms.Modules.Deals
{
    [BsonIgnoreExtraElements]
    public class DealModule : BaseModule, IModuleEntity
    {
        public static readonly Guid _id = Guid.Parse("da8d7734-3340-47a2-9291-20c6f890a5af");
        public Guid Id { get => _id; }
        public List<Deal> Deals { get; set; }
        public bool IsVertical { get; set; }
        [BsonIgnore]
        public String CityUrl { get; set; }
    }

    public class Deal
    {
        public string Images { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public double Price { get; set; }
        public UInt16 Order { get; set; }
        public double MinimumPrice { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
