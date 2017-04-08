using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [ForeignKey("Conversation")]
        public int ConversationID { get; set; }

        public Conversation Conversation { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Body { get; set; }

        public DateTime Created { get; set; }

        [ForeignKey("Author")]
        public string AuthorID { get; set; }

        public ApplicationUser Author { get; set; }

        public Message()
        {

        }
    }
}