using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class ServiceDetailsViewModel
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

        // ********** Staff Owner Info **********

        public string ForeName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public ICollection<ImageService> Images { get; set; } = new Collection<ImageService>();

        public ServiceDetailsViewModel(Service s)
        {
            ServiceID = s.ServiceID;
            Title = s.Title;
            SubTitle = s.SubTitle;
            Description = s.Description;
            ForeName = s.StaffOwner.Forename;
            Surname = s.StaffOwner.Surname;
            Email = s.StaffOwner.Email;
            Images = s.Images;
        }
    }
}