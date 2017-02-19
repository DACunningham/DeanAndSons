using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DeanAndSons.Models
{
    public class ImageProperty : Image
    {
        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public Property Property { get; set; }

        public ImageProperty() : base()
        {

        }

        public ImageProperty(HttpPostedFileBase file, ImageType imgType, string imgLocation, Property propObj) : base(imgType, imgLocation, file)
        {
            Property = propObj;
        }
    }
}
