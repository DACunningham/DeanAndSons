using DeanAndSons.Models.WAP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DeanAndSons.Models
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

        public string imgLocation = "/Storage/Propertys";

        protected Property()
        {
            Contact = new Collection<ContactProperty>();
            Images = new Collection<ImageProperty>();
        }

        public Property(PropertyCreateViewModel vm)
        {
            Title = vm.Title;
            Description = vm.Description;
            Type = vm.Type;
            Price = vm.Price;
            Contact = new Collection<ContactProperty>();
            Images = new Collection<ImageProperty>();

            Contact.Add(new ContactProperty(vm.PropertyNo, vm.Street, vm.Town, vm.PostCode, vm.TelephoneNo, vm.Email, this));
            Images = addImages(vm.Images);

        }

        //Checks if property's contact value is null
        public ContactProperty getContact(ICollection<ContactProperty> contactCol)
        {
            ContactProperty contact = null;
            try
            {
                contact = contactCol.First();
            }
            catch (InvalidOperationException)
            {
                contact = new ContactProperty();
                contact.PropertyNo = "No address found";
            }

            return contact;
        }

        //Takes a collectin of uploaded images and creates a new ImpageProperty for each non null && non empty entry
        private ICollection<ImageProperty> addImages(ICollection<HttpPostedFileBase> files)
        {
            var images = new Collection<ImageProperty>();
            ImageType imgType = ImageType.PropertyHeader;

            for (int i = 0; i < files.Count; i++)
            {
                if (files.ElementAt(i) != null && files.ElementAt(i).ContentLength != 0)
                {
                    if (i != 0)
                        imgType = ImageType.PropertyBody;

                    images.Add(new ImageProperty(files.ElementAt(i), imgType, imgLocation, this));
                }
            }

            return images;
        }
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
