using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.CMS.ViewModels
{
    public class PropertyEditCMSViewModel
    {
        public int PropertyID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        //********** Non-Editable **********
        public PropertyType Type { get; set; }

        public int Price { get; set; }

        public PropertyStyle Style { get; set; }

        public PropertyAge Age { get; set; }

        public int NoBedRms { get; set; }

        public int NoBathRms { get; set; }

        public int NoSittingRms { get; set; }

        //********** Image **********

        public ICollection<ImageProperty> ImagesProp { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }

        public PropertyEditCMSViewModel()
        {

        }

        public PropertyEditCMSViewModel(Property obj)
        {
            PropertyID = obj.PropertyID;
            Title = obj.Title;
            Description = obj.Description;
            ImagesProp = obj.Images;
            Type = obj.Type;
            Age = obj.Age;
            Style = obj.Style;
            Price = obj.Price;
            NoBedRms = obj.NoBedRms;
            NoBathRms = obj.NoBathRms;
            NoSittingRms = obj.NoSittingRms;
        }
    }
}