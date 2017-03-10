using DeanAndSons.Models.WAP;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models
{
    public class Staff : ApplicationUser
    {
        // Foreign Key for superior object
        [ForeignKey("Superior")]
        public string SuperiorID { get; set; }

        // Staff member who is the superior of this user
        public Staff Superior { get; set; }

        // List of subordinates of this user
        public ICollection<Staff> Subordinates { get; set; }

        public ICollection<Service> ServicesOwned { get; set; }

        public ICollection<Event> EventsOwned { get; set; }

        public ICollection<Property> PropertysOwned { get; set; }
    }
}