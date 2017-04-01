using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class RegisterStaffViewModel
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Your {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Your {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        public Rank Rank { get; set; }

        [ForeignKey("Superior")]
        public string SuperiorID { get; set; }

        public Staff Superior { get; set; }

        public List<string> SubordinateIds { get; set; } = new List<string>();

        [Display(Name = "Subordinates")]
        public MultiSelectList Subordinates { get; set; }

        public RegisterStaffViewModel()
        {

        }
    }
}