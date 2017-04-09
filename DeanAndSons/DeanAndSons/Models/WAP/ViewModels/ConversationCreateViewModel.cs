using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class ConversationCreateViewModel
    {
        [Display(Name = "Recipient")]
        public string ReceiverID { get; set; }

        [ForeignKey("ReceiverID")]
        [Display(Name = "Recipient")]
        public ApplicationUser Receiver { get; set; }

        //********** Message Fields **********

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        [Display(Name = "Message")]
        public string Body { get; set; }
    }
}