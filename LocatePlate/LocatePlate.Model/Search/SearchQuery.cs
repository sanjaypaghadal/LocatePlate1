using LocatePlate.Infrastructure.Domain;
using System;

namespace LocatePlate.Model.Search
{
    public class SearchQuery : ClientSide
    {
        public string query { get; set; }
        public string DealUrl { get; set; }
        public string Tags { get; set; }
        public Guid? location { get; set; }
        public string Cuisine { get; set; } 
        public string Categories { get; set; } 
        //public string MealType { get; set; } = string.Empty;
        public string FoodType { get; set; } 
        public int PageNumber { get; set; }
        public int RowsOfPage { get; set; }
        public string SortingCol { get; set; }
        public int SortType { get; set; }
        public int PartySize { get; set; } = 2;
        public Decimal? Logitude { get; set; }
        public Decimal? Latitude{ get; set; }
        public DateTime SearchDate { get; set; } = DateTime.UtcNow;
        public int SeatingPreference { get; set; }
    }
}
