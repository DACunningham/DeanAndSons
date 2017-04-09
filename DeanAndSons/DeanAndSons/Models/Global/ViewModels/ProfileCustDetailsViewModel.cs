using DeanAndSons.Models.WAP;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeanAndSons.Models.Global.ViewModels
{

    public class ProfileCustDetailsViewModel
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

        // Min property price range
        [Display(Name = "Lower Budget")]
        public int? BudgetLower { get; set; }

        // Max property price range
        [Display(Name = "Upper Budget")]
        public int? BudgetHigher { get; set; }

        [Display(Name = "Preferred Property Type")]
        public PropertyType PrefPropertyType { get; set; }

        [Display(Name = "Preferred Property Style")]
        public PropertyStyle PrefPropertyStyle { get; set; }

        [Display(Name = "Preferred Property Age")]
        public PropertyAge PrefPropertyAge { get; set; }

        [Display(Name = "Preferred Number of Bed Rooms")]
        public int PrefNoBedRms { get; set; }

        [Display(Name = "Preferred Number of Bathrooms")]
        public int PrefNoBathRms { get; set; }

        [Display(Name = "Preferred Number of Sitting Rooms")]
        public int PrefNoSittingRms { get; set; }

        public ContactUser Contact { get; set; }
        public ImageAppUser Image { get; set; }
        public ICollection<SavedSearch> SavedSearches { get; set; }
        public ICollection<Property> SavedProperties { get; set; }
        public string CurrentUserID { get; set; }

        public ProfileCustDetailsViewModel(Customer usr)
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

            Contact = usr.getContact(usr.Contact);
            Image = usr.getImage(usr.Image);
            SavedSearches = usr.SavedSearches;
            SavedProperties = usr.SavedPropertys;
        }
    }
}