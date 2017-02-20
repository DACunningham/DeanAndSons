using System.Collections.Generic;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class PropertyDetailsViewModel
    {
        public int PropertyID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PropertyType Type { get; set; }

        public int Price { get; set; }

        //********** Images **********
        public ICollection<ImageProperty> Images { get; set; }

        //********** Contact **********
        public string PropertyNo { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        private ContactProperty _contact;

        public PropertyDetailsViewModel()
        {

        }

        public PropertyDetailsViewModel(Property property)
        {
            PropertyID = property.PropertyID;
            Title = property.Title;
            Description = property.Description;
            Type = property.Type;
            Price = property.Price;

            Images = property.Images;

            _contact = property.getContact(property.Contact);
            PropertyNo = _contact.PropertyNo;
            Street = _contact.Street;
            Town = _contact.Town;
            PostCode = _contact.PostCode;
        }
    }
}