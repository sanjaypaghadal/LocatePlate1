using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.RestaurantDomain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Model.Cms
{
    public class Page : BaseEntityMongoDB
    {
        [Required]
        public int Country { get; set; } = 38;
        [Required]
        public int Province { get; set; } = 9;
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Url { get; set; }
        [MaxLength(2000)]
        public string Header { get; set; }
        public bool IsNewRestaurantShow{ get; set; }
        public List<Restaurant> NewRestaurants { get; set; }
        public string MetaData { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Logitude { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Latitude { get; set; }
        [MaxLength(2000)]
        public string Css { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal ProvinceTax { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal InternetTax { get; set; }
        public PageLayout PageLayout { get; set; }

    }
}
