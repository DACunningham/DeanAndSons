using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public class Contact
    {
        public int ContactID { get; set; }
        public int PropertyNo { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public int TelephoneNo { get; set; }
        public string Email { get; set; }


    }
}
