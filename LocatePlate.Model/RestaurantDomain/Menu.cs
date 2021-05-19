using LocatePlate.Infrastructure.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Menu : BaseEntity
    {
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double Price { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR")]
        public string Images { get; set; } = string.Empty;
        [NotMapped]
        public string MobileImages
        {
            get
            {
                if (!string.IsNullOrEmpty(Images))
                    return $"/UploadImages/{UserId}/{RestaurantId}/Menu/{Images}";
                return string.Empty;
            }
        }
        public string About { get; set; }
        [Column(TypeName = "VARCHAR")]
        public string Recipes { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Calories { get; set; }
        [Display(Name = "Food Nature")]
        public FoodNature FoodNature { get; set; }
        public int MenuCategoryId { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string MenuCategoryName { get; set; }
        [MaxLength(50)]
        [Display(Name = "Food Nature Name")]
        [Column(TypeName = "VARCHAR")]
        public string FoodNatureName { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string RestaurantName { get; set; }
        public override int RestaurantId { get; set; }
        public MenuCategory MenuCategory { get; set; }
        public Restaurant Restaurant { get; set; }
        [NotMapped]
        public virtual bool IsAddedToCart { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public bool IsMenuReservation { get; set; }
    }

    public enum FoodNature
    {
        [Display(Name = "Vegan")] Vegan,
        [Display(Name = "Vegetarian")] Vegetarian,
        [Display(Name = "Non Vegetarian")] NonVegetarian
    }
}
