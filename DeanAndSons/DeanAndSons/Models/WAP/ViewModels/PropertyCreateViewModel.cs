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
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public PropertyType Type { get; set; }

        public int Price { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }


        //********** Image **********
        public string Location { get; set; }
        public ImageType ImgType { get; set; }


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