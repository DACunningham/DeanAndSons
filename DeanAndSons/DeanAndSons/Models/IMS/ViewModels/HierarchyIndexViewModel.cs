using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class HierarchyIndexViewModel
    {
        public string StaffID { get; set; }

        public Staff Superior { get; set; }

        public ICollection<Staff> Subordinates { get; set; }

        public HierarchyIndexViewModel(Staff staff)
        {
            StaffID = staff.Id;
            Superior = staff.Superior;
            Subordinates = staff.Subordinates;
        }
    }
}