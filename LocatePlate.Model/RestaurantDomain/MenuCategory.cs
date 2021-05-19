using LocatePlate.Infrastructure.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class MenuCategory : BaseEntity
    {
        [MaxLength(30)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }
        public ICollection<Menu> Menus { get; set; }
    }
}
