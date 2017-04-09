using System.ComponentModel.DataAnnotations;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class MessageCreateViewModel
    {
        public int ConversationID { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Body { get; set; }

        public MessageCreateViewModel()
        {

        }

        public MessageCreateViewModel(int convID)
        {
            ConversationID = convID;
        }
    }
}