using DeanAndSons.Models.WAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.Global.ViewModels
{
    public class ProfileStaffDetailsViewModel
    {
        public string ID { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        // Information about this user
        public string About { get; set; }

        public string Email { get; set; }

        // The user name that should be displayed around the site (not used for validation)
        [Display(Name = "User Name")]
        public string UserNameDisp { get; set; }

        public ICollection<Service> ServicesOwned { get; set; }

        public ICollection<Event> EventsOwned { get; set; }

        public ICollection<Property> PropertysOwned { get; set; }

        public ContactUser Contact { get; set; }

        public ImageAppUser Image { get; set; }

        public ProfileStaffDetailsViewModel(Staff usr)
        {
            ID = usr.Id;
            Forename = usr.Forename;
            Surname = usr.Surname;
            About = usr.About;
            Email = usr.Email;
            UserNameDisp = usr.UserNameDisp;
            ServicesOwned = usr.ServicesOwned;
            EventsOwned = usr.EventsOwned;
            PropertysOwned = usr.PropertysOwned;

            Contact = usr.getContact(usr.Contact);
            Image = usr.getImage(usr.Image);
        }
    }
}