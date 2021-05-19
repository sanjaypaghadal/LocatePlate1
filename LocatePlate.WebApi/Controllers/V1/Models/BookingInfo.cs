using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.V1.Models
{
    public class BookingInfo
    {
        public List<Capacity> PartyCapacity { get; set; }
        public List<DateTime> Slots { get; set; }

    }
}
