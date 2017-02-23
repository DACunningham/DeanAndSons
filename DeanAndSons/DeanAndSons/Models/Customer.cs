using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models
{
    public class Customer : ApplicationUser
    {
        [Display(Name = "Lower Budget")]
        public int? BudgetLower { get; set; }

        [Display(Name = "Upper Budget")]
        public int? BudgetHigher { get; set; }

        [InverseProperty("Buyer")]
        public ICollection<Property> PropertysBuy { get; set; }

        [InverseProperty("Seller")]
        public ICollection<Property> PropertysSell { get; set; }
    }
}