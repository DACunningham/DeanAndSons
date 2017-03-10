using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class ContactEvent : Contact
    {
        [ForeignKey("Event")]
        public int EventID { get; set; }

        public Event Event { get; set; }

        public ContactEvent() : base()
        {

        }

        public ContactEvent(string propertyNo, string street, string town, string postCode, int? telephoneNo, string email, Event evObj)
            : base(propertyNo, street, town, postCode, telephoneNo, email)
        {
            Event = evObj;
        }
    }
}