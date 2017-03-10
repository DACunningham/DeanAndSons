using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class ImageService
    {
        [ForeignKey("Service")]
        public string ServiceID { get; set; }

        public Service Service { get; set; }
    }
}