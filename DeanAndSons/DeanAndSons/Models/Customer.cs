using DeanAndSons.Models.WAP;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models
{
    public class Customer : ApplicationUser
    {
        // Min property price range
        [Range(50000, 100000000, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Lower Budget")]
        public int? BudgetLower { get; set; } = 50000;

        // Max property price range
        [Range(50000, 100000000, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Upper Budget")]
        public int? BudgetHigher { get; set; } = 1000000;

        [Display(Name = "Preferred Property Type")]
        public PropertyType PrefPropertyType { get; set; } = PropertyType.Any;

        [Display(Name = "Preferred Property Style")]
        public PropertyStyle PrefPropertyStyle { get; set; } = PropertyStyle.Any;

        [Display(Name = "Preferred Property Age")]
        public PropertyAge PrefPropertyAge { get; set; } = PropertyAge.Any;

        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Preferred Number of Bed Rooms")]
        public int PrefNoBedRms { get; set; } = 1;

        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Preferred Number of Bathrooms")]
        public int PrefNoBathRms { get; set; } = 1;

        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Preferred Number of Sitting Rooms")]
        public int PrefNoSittingRms { get; set; } = 1;

        // List of properties this user has bought
        [InverseProperty("Buyer")]
        public ICollection<Property> PropertysBuy { get; set; } = new Collection<Property>();

        // List of properties this user has sold
        [InverseProperty("Seller")]
        public ICollection<Property> PropertysSell { get; set; } = new Collection<Property>();

        //public ICollection<Service> Serivces { get; set; }
    }
}