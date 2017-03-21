using DeanAndSons.Models.WAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.Global.ViewModels
{
    public class ProfileStaffEditViewModel
    {
        public string ID { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        // Information about this user
        public string About { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        // The user name that should be displayed around the site (not used for validation)
        [Display(Name = "User Name")]
        public string UserNameDisp { get; set; }

        //********** Contact **********
        [Required]
        [Display(Name = "Property Number/Name")]
        public string PropertyNo { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Telephone Number")]
        public int? TelephoneNo { get; set; }

        //********** Image **********
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        public ProfileStaffEditViewModel()
        {

        }

        public ProfileStaffEditViewModel(Staff usr)
        {
            ID = usr.Id;
            Forename = usr.Forename;
            Surname = usr.Surname;
            About = usr.About;
            Email = usr.Email;
            UserNameDisp = usr.UserNameDisp;

            ContactUser addr = usr.getContact(usr.Contact);

            PropertyNo = addr.PropertyNo;
            Street = addr.Street;
            Town = addr.Town;
            PostCode = addr.PostCode;
            TelephoneNo = addr.TelephoneNo;
        }
    }
}