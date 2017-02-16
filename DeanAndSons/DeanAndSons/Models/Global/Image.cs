using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeanAndSons.Models
{
    public abstract class Image
    {
        public int ImageID { get; set; }
        public string Location { get; set; }
        public ImageType Type { get; set; }
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
