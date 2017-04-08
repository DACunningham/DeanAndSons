using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }

        public string SenderID { get; set; }

        public string ReceiverID { get; set; }

        [ForeignKey("SenderID")]
        public ApplicationUser Sender { get; set; }

        [ForeignKey("ReceiverID")]
        public ApplicationUser Receiver { get; set; }

        public ICollection<Message> Messages { get; set; }

        [Required]
        public DateTime LastNewMessage { get; set; }

        [Required]
        public DateTime LastCheckedSender { get; set; }

        [Required]
        public DateTime LastCheckedReceiver { get; set; }

        public Conversation()
        {

        }
    }
}