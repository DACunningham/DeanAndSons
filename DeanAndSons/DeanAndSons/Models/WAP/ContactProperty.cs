using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public class ContactProperty
    {
        //public int ContactPropertyID { get; set; }
        public int PropertyID { get; set; }
        public int ContactID { get; set; }

        public Property Property { get; set; }
        public Contact Contact { get; set; }
    }
}
