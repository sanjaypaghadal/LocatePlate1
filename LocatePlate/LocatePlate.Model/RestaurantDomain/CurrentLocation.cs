using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.Model.RestaurantDomain
{
    public class CurrentLocation
    {
        [Key]
        public int Id { get; set; }
        public Guid LocationId { get; set; }
        public string CityName { get; set; }
        public decimal Distance { get; set; }

    }
}
