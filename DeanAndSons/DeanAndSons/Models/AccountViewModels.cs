using DeanAndSons.Models.WAP;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeanAndSons.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // *********** Custom Customer Info **********

        [Required]
        [StringLength(60, ErrorMessage = "Your {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Your {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        // *********** END Custom Customer Info **********
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    // *********** Custom View Models **********

    public class ProfileDetailsViewModel
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

        public ProfileDetailsViewModel(Customer usr)
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
        }
    }
}
