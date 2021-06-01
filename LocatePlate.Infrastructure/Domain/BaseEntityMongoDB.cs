using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Infrastructure.Domain
{
    public class BaseEntityMongoDB
    {
        [BsonId]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsSoftDelete { get; set; }
        public Guid UserId { get; set; }
    }
}
