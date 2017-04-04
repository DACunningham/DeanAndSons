using DeanAndSons.Models.WAP;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Models.CMS.ViewModels
{
    public class ServiceEditCMSViewModel
    {
        public int ServiceID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        //********** Non-Editable **********

        public string Forename { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        //********** Image **********

        public ICollection<ImageService> ImagesProp { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<HttpPostedFileBase> Images { get; set; }

        public ServiceEditCMSViewModel()
        {

        }

        public ServiceEditCMSViewModel(Service obj)
        {
            ServiceID = obj.ServiceID;
            Title = obj.Title;
            Description = obj.Description;
            ImagesProp = obj.Images;

            Forename = obj.StaffOwner.Forename;
            Surname = obj.StaffOwner.Surname;
            Email = obj.StaffOwner.Email;
        }
    }
}