using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    public class Property
    {
        public int PropertyID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PropertyType Type { get; set; }

        public ContactProperty Contact { get; set; }
        public ICollection<ImageProperty> Images { get; set; }
    }

    public enum PropertyType
    {
        House = 1,
        Flat,
        Maissonette,
        Mobile,
        Bungalow,
        HMO
    }
}
