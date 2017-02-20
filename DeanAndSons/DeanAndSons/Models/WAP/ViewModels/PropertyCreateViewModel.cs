using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class PropertyCreateViewModel
    {
        [Key]
        public int PropertyID { get; set; }

        [Required]
        [MaxLength(75), MinLength(10)]
        public string Title { get; set; }

        [Required]
        [MaxLength(3000), MinLength(10)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public PropertyType Type { get; set; }

        [Required]
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
        [Display(Name = "No# Bed Rooms")]
        public int NoBedRms { get; set; }

        [Required]
        [Display(Name = "No# Bath Rooms")]
        public int NoBathRms { get; set; }

        [Required]
        [Display(Name = "No# Sitting Rooms")]
        public int NoSittingRms { get; set; }

        //********** Image **********
        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }

        //********** Users **********
        public ApplicationUser Buyer { get; set; }

        public ApplicationUser Seller { get; set; }

        //********** Contact **********
        [Required]
        [Display(Name = "Property Number/Name")]
        public string PropertyNo { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Telephone Number")]
        public int? TelephoneNo { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}