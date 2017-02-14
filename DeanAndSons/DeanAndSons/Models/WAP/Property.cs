using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstateAgency.Models
{
    [Table("Propertys")]
    public class Property
    {
        public int PropertyID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public PropertyType Type { get; set; }

        public int Price { get; set; }

        //Only want to allow one of these programmatically
        public ICollection<ContactProperty> Contact { get; set; }
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
