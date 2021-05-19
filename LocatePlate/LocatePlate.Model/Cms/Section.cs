using LocatePlate.Infrastructure.Domain;
using LocatePlate.Model.Cms.Modules;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Model.Cms
{
    [BsonIgnoreExtraElements]
    public class Section : BaseEntityMongoDB
    {
        public Section()
        {

        }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Markup { get; set; }
        [MaxLength(20)]
        public string Image { get; set; }
        [Required]
        public DeviceType DeviceType { get; set; }
        public Module Module { get; set; }

    }
    public enum DeviceType
    {
        Browser,
        Desktop,
        Mobile
    }
}
