using DeanAndSons.Models.WAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Models.Global.ViewModels
{
    public class ProfileStaffEditViewModel
    {
        public string ID { get; set; }

        // User's first name
        [Required]
        [StringLength(60, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        // User's second name
        [Required]
        [StringLength(60, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        // Information about this user
        public string About { get; set; }

        // The user name that should be displayed around the site (not used for validation)
        [Required]
        [Display(Name = "User Name to Display")]
        public string UserNameDisp { get; set; }

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

        //********** Image **********
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        [ForeignKey("SiteThemeObj")]
        public string SiteTheme { get; set; }

        public SelectList SiteThemeObj { get; set; }

        public ProfileStaffEditViewModel()
        {

        }

        public ProfileStaffEditViewModel(Staff usr)
        {
            ID = usr.Id;
            Forename = usr.Forename;
            Surname = usr.Surname;
            About = usr.About;
            UserNameDisp = usr.UserNameDisp;

            ContactUser addr = usr.getContact(usr.Contact);

            PropertyNo = addr.PropertyNo;
            Street = addr.Street;
            Town = addr.Town;
            PostCode = addr.PostCode;
            TelephoneNo = addr.TelephoneNo;
            SiteTheme = usr.SiteTheme;
        }
    }
}