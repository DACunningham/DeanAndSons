using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DeanAndSons.Models.WAP
{
    public class ImageService : Image
    {
        [ForeignKey("Service")]
        public string ServiceID { get; set; }

        public Service Service { get; set; }

        public ImageService() : base()
        {

        }

        public ImageService(HttpPostedFileBase file, ImageType imgType, string imgLocation, Service svcObj) : base(imgType, imgLocation, file)
        {
            Service = svcObj;
        }
    }
}