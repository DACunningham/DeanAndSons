using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class Service
    {
        public int ServiceID { get; set; }

        public string Title { get; set; }

        public string About { get; set; }

        public string StaffOwnerID { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        public ICollection<ImageService> Images { get; set; }
    }
}