using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class PropertyEditCMSViewModel
    {
        public int PropertyID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        //********** Image **********
        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }

        public PropertyEditCMSViewModel()
        {

        }

        public PropertyEditCMSViewModel(Property obj)
        {
            Title = obj.Title;
            Description = obj.Description;
        }
    }
}