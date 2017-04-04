using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class StaffManagerEditViewModel
    {
        public string Id { get; set; }

        public string Forename { get; set; }

        [Required]
        public Rank Rank { get; set; }

        public string SuperiorID { get; set; }

        public Staff Superior { get; set; }

        public List<string> SubordinateIds { get; set; } = new List<string>();

        [Display(Name = "Subordinates")]
        public MultiSelectList Subordinates { get; set; }

        public StaffManagerEditViewModel(Staff vm)
        {
            Id = vm.Id;
            Forename = vm.Forename;
            Rank = vm.Rank;
            SuperiorID = vm.SuperiorID;
            Superior = vm.Superior;
            getSubordinateIDs(vm.Subordinates);
        }

        public StaffManagerEditViewModel()
        {

        }

        private void getSubordinateIDs(ICollection<Staff> subordinates)
        {
            foreach (var item in subordinates)
            {
                SubordinateIds.Add(item.Id);
            }
        }
    }
}