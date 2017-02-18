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
            Contact = getContact(item);
            Description = createDescription(item.Description);
            Image = getImage(item);
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

        private ContactProperty getContact(Property item)
        {
            ContactProperty contact = null;
            try
            {
                contact = item.Contact.First();
            }
            catch (InvalidOperationException)
            {
                contact = new ContactProperty();
            }

            return contact;
        }

        private ImageProperty getImage(Property item)
        {
            ImageProperty image = null;
            try
            {
                image = item.Images.First(i => i.Type == ImageType.PropertyHeader);
            }
            catch (InvalidOperationException)
            {
                image = new ImageProperty();
            }

            return image;
        }
    }
}