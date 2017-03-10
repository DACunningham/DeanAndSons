using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class ImageAppUser : Image
    {
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}