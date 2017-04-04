using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class ServiceCreateIMSViewModel
    {
        public int ServiceID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string SubTitle { get; set; } = "**********Placeholder text to be replaced in CMS**********";

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Description { get; set; } = "**********Placeholder text to be replaced in CMS**********";

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Required]
        public string StaffOwnerID { get; set; }

        public SelectList StaffOwner { get; set; }
    }
}