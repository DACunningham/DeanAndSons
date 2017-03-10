using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class ImageUser : Image
    {
        [ForeignKey("Customer")]
        public string UserID { get; set; }

        public Customer Customer { get; set; }
    }
}