using DeanAndSons.Models.WAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DeanAndSons.Models.CMS.ViewModels
{
    public class EventEditCMSViewModel
    {
        public int EventID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        //********** Non-Editable **********

        public string PropertyNo { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        //********** Image **********

        public ICollection<ImageEvent> ImagesProp { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }

        public EventEditCMSViewModel()
        {

        }

        public EventEditCMSViewModel(Event obj)
        {
            EventID = obj.EventID;
            Title = obj.Title;
            Description = obj.Description;
            ImagesProp = obj.Images;

            var _contact = obj.getContact(obj.Contact);
            PropertyNo = _contact.PropertyNo;
            Street = _contact.Street;
            Town = _contact.Town;
            PostCode = _contact.PostCode;

            Forename = obj.StaffOwner.Forename;
            Surname = obj.StaffOwner.Surname;
            Email = obj.StaffOwner.Email;

            Created = obj.Created;
        }
    }
}