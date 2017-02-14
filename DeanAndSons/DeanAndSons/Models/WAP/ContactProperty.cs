using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public class ContactProperty : Contact
    {
        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public Property Property { get; set; }

        public ContactProperty() : base()
        {

        }
    }
}