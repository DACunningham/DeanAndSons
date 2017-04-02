using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class PropertyEditIMSViewModel
    {
        public int PropertyID { get; set; }

        public string Title { get; set; }

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
        public DateTime LastModified { get; set; } = DateTime.Now;

        [ForeignKey("Buyer")]
        public string BuyerID { get; set; }

        [ForeignKey("Seller")]
        public string SellerID { get; set; }

        public string StaffOwnerID { get; set; }

        public SelectList Buyer { get; set; }

        public SelectList Seller { get; set; }

        public SelectList StaffOwner { get; set; }

        public PropertyEditIMSViewModel()
        {

        }

        public PropertyEditIMSViewModel(Property obj)
        {
            PropertyID = obj.PropertyID;
            Title = obj.Title;
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
}