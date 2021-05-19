using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Model.Search
{
    public class FilterCriteria
    {
        public List<StringModel> Cuisine { get; set; } = new List<StringModel>();
        public List<StringModel> RestaurantType { get; set; } = new List<StringModel>();
        //public List<StringModel> MealType { get; set; } = new List<StringModel>();
        public List<StringModel> FoodNature { get; set; } = new List<StringModel>();
    }

    public enum SortColumn
    {
        Popularity = 0,
        Rating = 1,
        Nearest = 2,
        Farest = 3,
        [Display(Name = "Cost low to high")]
        CostLowToHigh = 4,
        [Display(Name = "Cost high to low")]
        CostHighToLow = 5
    }
}