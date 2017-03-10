using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class Event
    {
        public int EventID { get; set; }

        [Required]
        [MaxLength(75), MinLength(5)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000), MinLength(5)]
        public string Description { get; set; }

        [Required]
        public string StaffOwnerID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        public ICollection<ImageEvent> Images { get; set; }
    }
}