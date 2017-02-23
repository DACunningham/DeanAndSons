using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models
{
    public class Staff : ApplicationUser
    {
        [ForeignKey("Superior")]
        public string SuperiorID { get; set; }

        public Staff Superior { get; set; }

        public ICollection<Staff> Subordinates { get; set; }
    }
}