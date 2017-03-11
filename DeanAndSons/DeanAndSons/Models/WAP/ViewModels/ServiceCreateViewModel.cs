using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class ServiceCreateViewModel
    {
        [Key]
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

        [Required]
        public string StaffOwnerID { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        //********** Image **********
        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }
    }
}