using DeanAndSons.Models.WAP;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class ServiceEditIMSViewModel
    {
        public int ServiceID { get; set; }

        public string Title { get; set; }

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Required]
        public string StaffOwnerID { get; set; }

        public SelectList StaffOwner { get; set; }

        public ServiceEditIMSViewModel()
        {

        }

        public ServiceEditIMSViewModel(Service obj)
        {
            ServiceID = obj.ServiceID;
            Title = obj.Title;
            StaffOwnerID = obj.StaffOwnerID;
        }
    }
}