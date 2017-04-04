using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.WAP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Models
{
    [Table("Propertys")]
    public class Property
    {
        public int PropertyID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public PropertyType Type { get; set; }

        [Required]
        [Range(50000, 100000000, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
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
        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "No# Bed Rooms")]
        public int NoBedRms { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "No# Bath Rooms")]
        public int NoBathRms { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a value of between {1} and {2} for {0}.")]
        [Display(Name = "No# Sitting Rooms")]
        public int NoSittingRms { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Required]
        public bool Deleted { get; set; } = false;

        [ForeignKey("Buyer")]
        public string BuyerID { get; set; }

        [ForeignKey("Seller")]
        public string SellerID { get; set; }

        public string StaffOwnerID { get; set; }

        //Only want to allow one of these programmatically
        public ICollection<ContactProperty> Contact { get; set; } = new Collection<ContactProperty>();

        public ICollection<ImageProperty> Images { get; set; } = new Collection<ImageProperty>();

        public Customer Buyer { get; set; }

        public Customer Seller { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        public string imgLocation = "/Storage/Propertys";

        public Property()
        {

        }

        public Property(PropertyCreateIMSViewModel vm)
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
            StaffOwnerID = vm.StaffOwnerID;

            Contact.Add(new ContactProperty(vm.PropertyNo, vm.Street, vm.Town, vm.PostCode, vm.TelephoneNo, vm.Email, this));
            //Images = addImages(vm.Images);

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
        public ICollection<ImageProperty> addImages(ICollection<HttpPostedFileBase> files)
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

        /// <summary>
        /// Remove image from file store on server only.  Doesn't remove item from property collection
        /// or DB
        /// </summary>
        /// <param name="img">The image to remove</param>
        public void removeImage(ImageProperty img)
        {
            img.DeleteImage(img);
        }

        public void ApplyEditIMS(PropertyEditIMSViewModel obj)
        {
            Type = obj.Type;
            Price = obj.Price;
            SaleState = obj.SaleState;
            Style = obj.Style;
            Age = obj.Age;
            NoBedRms = obj.NoBedRms;
            NoBathRms = obj.NoBathRms;
            NoSittingRms = obj.NoSittingRms;
            BuyerID = obj.BuyerID;
            SellerID = obj.SellerID;
            StaffOwnerID = obj.StaffOwnerID;
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
