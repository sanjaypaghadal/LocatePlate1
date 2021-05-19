using LocatePlate.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Ratings : BaseEntity
    {
        public double Rating { get; set; }
        public RatingType RatingType { get; set; }

        public IEnumerable<object> GroupBy(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }

    public enum RatingType
    {
        [Display(Name = "Ambience")] Ambience,
        [Display(Name = "Food")] Food,
        [Display(Name = "Service")] Service,
        [Display(Name = "Value")] Value
    }

    public enum RatingValue
    {
        [Display(Name = "Bad")] Bad=0,
        [Display(Name = "Mediocre")] Mediocre=1,
        [Display(Name = "Good")] Good=2,
        [Display(Name = "Awesome")] Awesome=3
    }
}
