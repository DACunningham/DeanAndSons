using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DeanAndSons.Models
{
    public abstract class Image
    {
        public int ImageID { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public ImageType Type { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime LastModified { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //*********** Static Public Fields *********
        public static string defaultImgLocation = "/Storage/Global/NoImg.jpg";

        public Image()
        {

        }

        public Image(ImageType type, string imgLocation, HttpPostedFileBase file)
        {
            Location = saveImage(imgLocation, file);
            Type = type;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
            Deleted = false;
        }

        private string saveImage(string path, HttpPostedFileBase file)
        {
            //Generate GUID filename.
            string saveFileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
            //Generate local file system string location so, the app can physically save the file to disk
            string fullPath = Path.Combine(HttpContext.Current.Server.MapPath(path), saveFileName);
            //Generate the relative path of the image in the app for DB entry
            string loc = path + "/" + saveFileName;

            try
            {
                //Save the actual file to the server by combining server path, images application path and the photo's name.
                file.SaveAs(fullPath);

                return loc;
            }
            catch (Exception)
            {

                throw new Exception("The image was not saved for some reason");
            }
        }
    }

    public enum ImageType
    {
        PropertyHeader = 1,
        PropertyBody,
        Profile,
        Event,
        Service
    }
}
