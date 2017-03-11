using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class EventDetailsViewModel
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        //********** Images **********
        public ICollection<ImageEvent> Images { get; set; }

        //********** Contact **********
        public string PropertyNo { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        //********** Staff Owner **********
        public string Forename { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }


        private ContactEvent _contact;

        public EventDetailsViewModel(Event e)
        {
            EventID = e.EventID;
            Title = e.Title;
            Description = e.Description;
            Created = e.Created;
            Images = e.Images;

            _contact = e.getContact(e.Contact);

            PropertyNo = _contact.PropertyNo;
            Street = _contact.Street;
            Town = _contact.Town;
            PostCode = _contact.PostCode;

            Forename = e.StaffOwner.Forename;
            Surname = e.StaffOwner.Surname;
            Email = e.StaffOwner.Email;
        }
    }
}