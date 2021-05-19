using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.Search
{
    public class SearchRecords
    {
        public List<string> FoodType { get; set; }
        public List<string> Dish { get; set; }
        public List<string> FoodCategory { get; set; }
        public List<SearchResult> SearchResults { get; set; }
        public string CityName { get; set; }
    }
    public class SearchResult
    {
        public Guid Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public string FoodType { get; set; }
        public string FoodCategory { get; set; }
        public string Url { get; set; }
        public string Dish { get; set; }
        public Guid LocationId { get; set; }
        public string Images { get; set; }
        public string Cuisine { get; set; }
        public double CostForTwo { get; set; }
        public string FullAddress { get; set; }
        public double? RatingCount { get; set; }
        public int ReviewCount { get; set; }
        public int AllCount { get; set; }
        public Guid UserId { get; set; }

        [NotMapped]
        public virtual List<string> Slots { get; set; } = null;

        public virtual string RestaurantUrl { get { return $"{CityName}/restaurant/{Url}".ToLower(); } }
    }


    public class StringModel
    {
        public Guid Id { get; set; }
        private string name;
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(Column1))
                    return Column1;
                else return name;
            }
            set
            {
                name = value;
            }
        }
        public string Column1 { get; set; }
    }
}
