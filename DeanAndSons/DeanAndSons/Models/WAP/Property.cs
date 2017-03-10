using DeanAndSons.Models.WAP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models
{
    [Table("Propertys")]
    public class Property
    {
        public int PropertyID { get; set; }

        [Required]
        [MaxLength(75), MinLength(10)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000), MinLength(10)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public PropertyType Type { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Sale Status")]
        public SaleState SaleState { get; set; }

        [Required]
        [Display(Name = "Property Style")]
        public PropertyStyle Style { get; set; }

        [Required]
        [Display(Name = "Property Age")]
        public PropertyAge Age { get; set; }

        [Required]
        [Display(Name = "No# Bed Rooms")]
        public int NoBedRms { get; set; }

        [Required]
        [Display(Name = "No# Bath Rooms")]
        public int NoBathRms { get; set; }

        [Required]
        [Display(Name = "No# Sitting Rooms")]
        public int NoSittingRms { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime LastModified { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [ForeignKey("Buyer")]
        public string BuyerID { get; set; }

        [ForeignKey("Seller")]
        public string SellerID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Only want to allow one of these programmatically
        public ICollection<ContactProperty> Contact { get; set; }

        public ICollection<ImageProperty> Images { get; set; }

        public Customer Buyer { get; set; }

        public Customer Seller { get; set; }

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
            SaleState = SaleState.ForSale;
            Style = vm.Style;
            Age = vm.Age;
            NoBedRms = vm.NoBedRms;
            NoBathRms = vm.NoBathRms;
            NoSittingRms = vm.NoSittingRms;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
            Deleted = false;

            Contact = new Collection<ContactProperty>();
            Images = new Collection<ImageProperty>();
            Contact.Add(new ContactProperty(vm.PropertyNo, vm.Street, vm.Town, vm.PostCode, vm.TelephoneNo, vm.Email, this));
            Images = addImages(vm.Images);

            Buyer = null;
            Seller = vm.Seller;
        }

        //Checks if property's contact value is null
        public ContactProperty getContact(ICollection<ContactProperty> contactCol)
        {
            ContactProperty contact = null;
            try
            {
                contact = contactCol.First();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException)
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
        Any = 0,
        House = 1,
        Flat = 2,
        Maissonette = 4,
        Mobile = 8,
        Bungalow = 16,
        HMO = 32,
        BuildingPlot = 64
    }

    public enum PropertyStyle
    {
        Any = 0,
        Detached = 1,
        SemiDetached = 2,
        Terraced = 4,
        EndTerrace = 8,
        GroundFloorFlat = 16,
        UpperFloorFlat = 32
    }

    [Flags]
    public enum PropertyAge
    {
        Any = 0,
        Older = 1,
        Modern = 2,
        NewBuild = 4,
        PostWar = 8,
        PreWar = 16
    }

    public enum SaleState
    {
        Any = 0,
        ForSale = 1,
        UnderOffer = 2,
        Sold = 4,
        ToLet = 8
    }
}
