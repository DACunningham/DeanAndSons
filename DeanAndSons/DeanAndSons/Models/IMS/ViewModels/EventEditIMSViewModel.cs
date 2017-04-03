using DeanAndSons.Models.WAP;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class EventEditIMSViewModel
    {
        public int EventID { get; set; }

        public string Title { get; set; }

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        public string StaffOwnerID { get; set; }

        public SelectList StaffOwner { get; set; }

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

        public EventEditIMSViewModel()
        {

        }

        public EventEditIMSViewModel(Event obj)
        {
            EventID = obj.EventID;
            Title = obj.Title;
            StaffOwnerID = obj.StaffOwnerID;

            var _contact = obj.getContact(obj.Contact);
            PropertyNo = _contact.PropertyNo;
            Street = _contact.Street;
            Town = _contact.Town;
            PostCode = _contact.PostCode;
            TelephoneNo = _contact.TelephoneNo;
            Email = _contact.Email;
        }
    }
}