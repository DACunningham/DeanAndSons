using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DeanAndSons.Models.WAP
{
    public class ImageAppUser : Image
    {
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ImageAppUser()
        {

        }

        public ImageAppUser(HttpPostedFileBase file, ImageType imgType, string imgLocation, ApplicationUser usrObj) : base(imgType, imgLocation, file)
        {
            ApplicationUser = usrObj;
        }
    }
}