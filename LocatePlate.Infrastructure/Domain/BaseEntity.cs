using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Infrastructure.Domain
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string CreatedBy { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsSoftDelete { get; set; }
        public Guid UserId { get; set; }
        public virtual int RestaurantId { get; set; }
        [NotMapped]
        public virtual string ClientZoneInfo { get; set; }
    }
}
