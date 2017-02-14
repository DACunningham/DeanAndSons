using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public abstract class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [Required]
        public int PropertyNo { get; set; }

        public string Street { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public int? TelephoneNo { get; set; }
        public string Email { get; set; }


    }
}
