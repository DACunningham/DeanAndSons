using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class EventCreateIMSViewModel
    {
        public int EventID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Description { get; set; } = "**********Placeholder text to be replaced in CMS**********";

        //********** Image **********
        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }

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

        public Staff StaffOwner { get; set; }
    }
}