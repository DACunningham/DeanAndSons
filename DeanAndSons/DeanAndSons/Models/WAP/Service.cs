﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class Service
    {
        public int ServiceID { get; set; }

        [Required]
        [MaxLength(75), MinLength(5)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000), MinLength(5)]
        public string Description { get; set; }

        [Required]
        public string StaffOwnerID { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        public ICollection<ImageService> Images { get; set; }
    }
}