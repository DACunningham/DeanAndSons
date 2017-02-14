using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public class ImageProperty : Image
    {
        public int PropertyID { get; set; }

        public Property Property { get; set; }
    }
}
