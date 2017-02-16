using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeanAndSons.Models
{
    public class ImageProperty : Image
    {
        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public Property Property { get; set; }
    }
}
