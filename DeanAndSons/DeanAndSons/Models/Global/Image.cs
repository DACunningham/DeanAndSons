using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DeanAndSons.Models
{
    public abstract class Image
    {
        public int ImageID { get; set; }
        public string Location { get; set; }
        public ImageType Type { get; set; }

        public Image()
        {

        }

        public Image(ImageType type, string imgLocation, HttpPostedFileBase file)
        {
            Location = saveImage(imgLocation, file);
            Type = type;
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
