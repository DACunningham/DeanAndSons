using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DeanAndSons.Models.WAP
{
    public class ImageEvent : Image
    {
        [ForeignKey("Event")]
        public string EventID { get; set; }

        public Event Event { get; set; }

        public ImageEvent() : base()
        {

        }

        public ImageEvent(HttpPostedFileBase file, ImageType imgType, string imgLocation, Event evObj) : base(imgType, imgLocation, file)
        {
            Event = evObj;
        }
    }
}