using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeanAndSons.Models
{
    public abstract class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [Required]
        [Display(Name = "Property Number/Name")]
        public string PropertyNo { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Telephone Number")]
        public int? TelephoneNo { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime LastModified { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Contact()
        {

        }

        public Contact(string propertyNo, string street, string town, string postCode, int? telephoneNo, string email)
        {
            PropertyNo = propertyNo;
            Street = street;
            Town = town;
            PostCode = postCode;
            TelephoneNo = telephoneNo;
            Email = email;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
            Deleted = false;
        }
    }
}
