using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class PropertyCreateIMSViewModel
    {
        [Key]
        public int PropertyID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; } = "**********Placeholder text to be replaced in CMS**********";

        [Required]
        [Display(Name = "Property Type")]
        public PropertyType Type { get; set; }

        [Required]
        [Range(50000, 100000000, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Sale Status")]
        public SaleState SaleState { get; set; }

        [Required]
        [Display(Name = "Property Style")]
        public PropertyStyle Style { get; set; }

        [Required]
        [Display(Name = "Property Age")]
        public PropertyAge Age { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "No# Bed Rooms")]
        public int NoBedRms { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "No# Bath Rooms")]
        public int NoBathRms { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "No# Sitting Rooms")]
        public int NoSittingRms { get; set; }

        //********** Users **********
        public string BuyerID { get; set; }
        public Customer Buyer { get; set; }

        [Required]
        public string SellerID { get; set; }
        public Customer Seller { get; set; }

        //********** Contact **********
        [Required]
        [StringLength(50, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Property Number/Name")]
        public string PropertyNo { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Street { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Town { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Telephone Number")]
        public int? TelephoneNo { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        //********** Staff Owner **********
        [Required]
        public string StaffOwnerID { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }
    }
}