using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models
{
    public class Customer : ApplicationUser
    {
        // Min property price range
        [Display(Name = "Lower Budget")]
        public int? BudgetLower { get; set; } = 50000;

        // Max property price range
        [Display(Name = "Upper Budget")]
        public int? BudgetHigher { get; set; } = 1000000;

        [Display(Name = "Preferred Property Type")]
        public PropertyType PrefPropertyType { get; set; } = PropertyType.Any;

        [Display(Name = "Preferred Property Style")]
        public PropertyStyle PrefPropertyStyle { get; set; } = PropertyStyle.Any;

        [Display(Name = "Preferred Property Age")]
        public PropertyAge PrefPropertyAge { get; set; } = PropertyAge.Any;

        [Display(Name = "Preferred Number of Bed Rooms")]
        public int PrefNoBedRms { get; set; } = 1;

        [Display(Name = "Preferred Number of Bathrooms")]
        public int PrefNoBathRms { get; set; } = 1;

        [Display(Name = "Preferred Number of Sitting Rooms")]
        public int PrefNoSittingRms { get; set; } = 1;

        // List of properties this user has bought
        [InverseProperty("Buyer")]
        public ICollection<Property> PropertysBuy { get; set; }

        // List of properties this user has sold
        [InverseProperty("Seller")]
        public ICollection<Property> PropertysSell { get; set; }
    }
}