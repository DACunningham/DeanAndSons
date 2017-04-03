using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DeanAndSons.Models.CMS.ViewModels
{
    public class ServiceCreateCMSViewModel
    {
        public int ServiceID { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(65, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string SubTitle { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Description { get; set; }

        //********** Image **********
        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }
    }
}