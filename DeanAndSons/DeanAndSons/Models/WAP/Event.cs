using DeanAndSons.Models.WAP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP
{
    public class Event
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
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Required]
        public bool Deleted { get; set; } = false;

        [Required]
        public string StaffOwnerID { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        public ICollection<ImageEvent> Images { get; set; } = new Collection<ImageEvent>();

        //Only want to allow one of these programmatically
        public ICollection<ContactEvent> Contact { get; set; } = new Collection<ContactEvent>();

        public string imgLocation = "/Storage/Events";

        public Event()
        {

        }

        public Event(EventCreateViewModel vm)
        {
            Title = vm.Title;
            Description = vm.Description;
            StaffOwnerID = vm.StaffOwnerID;
            Images = addImages(vm.Images);
            Contact.Add(new ContactEvent(vm.PropertyNo, vm.Street, vm.Town, vm.PostCode, vm.TelephoneNo, vm.Email, this));
        }

        //Checks if property's contact value is null
        public ContactEvent getContact(ICollection<ContactEvent> contactCol)
        {
            ContactEvent contact = null;
            try
            {
                contact = contactCol.First();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException)
            {
                contact = new ContactEvent();
                contact.PropertyNo = "No address found";
            }

            return contact;
        }

        //Takes a collectin of uploaded images and creates a new ImpageProperty for each non null && non empty entry
        private ICollection<ImageEvent> addImages(ICollection<HttpPostedFileBase> files)
        {
            var images = new Collection<ImageEvent>();
            ImageType imgType = ImageType.EventHeader;

            for (int i = 0; i < files.Count; i++)
            {
                if (files.ElementAt(i) != null && files.ElementAt(i).ContentLength != 0)
                {
                    if (i != 0)
                        imgType = ImageType.EventBody;

                    images.Add(new ImageEvent(files.ElementAt(i), imgType, imgLocation, this));
                }
            }

            return images;
        }
    }
}