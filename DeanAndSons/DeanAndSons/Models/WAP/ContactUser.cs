using DeanAndSons.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP
{
    public class ContactUser : Contact
    {
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ContactUser()
        {

        }

        public ContactUser(string propertyNo, string street, string town, string postCode, int? telephoneNo, string email, ApplicationUser userObj)
        {
            base.PropertyNo = propertyNo;
            base.Street = street;
            base.Town = town;
            base.PostCode = postCode;
            base.TelephoneNo = telephoneNo;
            base.Email = email;

            ApplicationUser = userObj;
        }
    }
}