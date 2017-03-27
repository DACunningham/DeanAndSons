﻿using DeanAndSons.Models.WAP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.Global.ViewModels
{
    public class ProfileCustEditViewModel
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

        [Range(50000, 100000000, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Lower Budget")]
        public int? BudgetLower { get; set; } = 50000;

        // Max property price range
        [Range(50000, 100000000, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Upper Budget")]
        public int? BudgetHigher { get; set; } = 1000000;

        [Display(Name = "Preferred Property Type")]
        public PropertyType PrefPropertyType { get; set; } = PropertyType.Any;

        [Display(Name = "Preferred Property Style")]
        public PropertyStyle PrefPropertyStyle { get; set; } = PropertyStyle.Any;

        [Display(Name = "Preferred Property Age")]
        public PropertyAge PrefPropertyAge { get; set; } = PropertyAge.Any;

        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Preferred Number of Bed Rooms")]
        public int PrefNoBedRms { get; set; } = 1;

        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Preferred Number of Bathrooms")]
        public int PrefNoBathRms { get; set; } = 1;

        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "Preferred Number of Sitting Rooms")]
        public int PrefNoSittingRms { get; set; } = 1;

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

        public ProfileCustEditViewModel()
        {

        }

        public ProfileCustEditViewModel(Customer usr)
        {
            ID = usr.Id;
            Forename = usr.Forename;
            Surname = usr.Surname;
            About = usr.About;
            Email = usr.Email;
            UserNameDisp = usr.UserNameDisp;
            BudgetLower = usr.BudgetLower;
            BudgetHigher = usr.BudgetHigher;
            PrefPropertyType = usr.PrefPropertyType;
            PrefPropertyStyle = usr.PrefPropertyStyle;
            PrefPropertyAge = usr.PrefPropertyAge;
            PrefNoBedRms = usr.PrefNoBedRms;
            PrefNoBathRms = usr.PrefNoBathRms;
            PrefNoSittingRms = usr.PrefNoSittingRms;

            ContactUser addr = usr.getContact(usr.Contact);
            PropertyNo = addr.PropertyNo;
            Street = addr.Street;
            Town = addr.Town;
            PostCode = addr.PostCode;
            TelephoneNo = addr.TelephoneNo;
        }
    }
}