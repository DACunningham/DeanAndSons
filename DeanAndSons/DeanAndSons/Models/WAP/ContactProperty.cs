﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeanAndSons.Models
{
    public class ContactProperty : Contact
    {
        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public Property Property { get; set; }

        public ContactProperty() : base()
        {

        }

        public ContactProperty(string propertyNo, string street, string town, string postCode, int? telephoneNo, string email, Property propObj)
            : base(propertyNo, street, town, postCode, telephoneNo, email)
        {
            Property = propObj;
        }
    }
}