using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public abstract class Image
    {
        public int ImageID { get; set; }
        public string Location { get; set; }
        public ImageType Type { get; set; }
    }

    public enum ImageType
    {
        Property = 1,
        Profile,
        Event,
        Service
    }
}
