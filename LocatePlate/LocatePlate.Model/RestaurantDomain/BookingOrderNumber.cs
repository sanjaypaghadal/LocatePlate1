using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.Model.RestaurantDomain
{
    public class BookingOrderNumber
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string BillId { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public int RestaurantId { get; set; }
        public long OrderNo { get; set; }
    }
}