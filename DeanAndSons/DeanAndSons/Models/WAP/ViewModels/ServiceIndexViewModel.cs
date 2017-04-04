using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class ServiceIndexViewModel
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(65, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string SubTitle { get; set; }

        public ImageService Image { get; set; }

        public ServiceIndexViewModel()
        {

        }

        public ServiceIndexViewModel(Service e)
        {
            ServiceID = e.ServiceID;
            Title = e.Title;
            SubTitle = e.SubTitle;
            Image = getImage(e);
        }

        /// <summary>
        /// Takes the description and cuts it down to 200 chars if required
        /// </summary>
        /// <param name="desc">The string to cut</param>
        /// <returns></returns>
        private string createDescription(string desc)
        {
            if (desc.Length > 200)
            {
                return desc.Substring(0, 200) + "...";
            }
            else
            {
                return desc + "...";
            }
        }

        //Checks if event's image value is null
        private ImageService getImage(Service item)
        {
            ImageService image = null;
            try
            {
                image = item.Images.First(i => i.Type == ImageType.ServiceHeader);
            }
            catch (InvalidOperationException)
            {
                image = new ImageService();
                image.Location = ImageService.defaultImgLocation;
            }

            return image;
        }
    }
}