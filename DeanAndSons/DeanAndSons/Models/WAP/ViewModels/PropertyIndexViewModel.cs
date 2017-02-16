using DeanAndSons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class PropertyIndexViewModel
    {
        public int PropertyID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PropertyType Type { get; set; }

        public int Price { get; set; }

        public ContactProperty Contact { get; set; }

        public ImageProperty Image { get; set; }

        public PropertyIndexViewModel(Property item)
        {
            Contact = item.Contact.SingleOrDefault();
            Description = createDescription(item.Description);
            Image = item.Images.Where(i => i.Type == ImageType.PropertyHeader).SingleOrDefault();
            Price = item.Price;
            PropertyID = item.PropertyID;
            Title = item.Title;
            Type = item.Type;
        }

        private string createDescription(string desc)
        {
            if (desc.Length > 200)
            {
                return desc.Substring(0, 200);
            }
            else
            {
                return desc;
            }
        }
    }
}