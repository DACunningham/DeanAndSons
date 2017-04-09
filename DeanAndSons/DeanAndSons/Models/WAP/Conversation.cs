using DeanAndSons.Models.WAP.ViewModels;
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

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        [Required]
        [Display(Name = "Last Message Sent")]
        public DateTime LastNewMessage { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Checked By Sender")]
        public DateTime LastCheckedSender { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Checked By Receiver")]
        public DateTime LastCheckedReceiver { get; set; } = DateTime.Now;

        public Conversation()
        {

        }

        public Conversation(ConversationCreateViewModel vm, string senderID)
        {
            ReceiverID = vm.ReceiverID;
            SenderID = senderID;
        }
    }
}