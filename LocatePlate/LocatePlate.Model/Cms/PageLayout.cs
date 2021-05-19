using LocatePlate.Infrastructure.Domain;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Model.Cms
{
    [BsonIgnoreExtraElements]
    public class PageLayout : BaseEntityMongoDB
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        public List<Section> Section { get; set; }
        public virtual Guid LocationId { get; set; }
    }

}

