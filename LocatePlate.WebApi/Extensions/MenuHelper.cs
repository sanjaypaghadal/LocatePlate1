using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Extensions
{
    public class MenuExtend : Menu
    {
        public string ImagesToDelete { get; set; }
    }
}
