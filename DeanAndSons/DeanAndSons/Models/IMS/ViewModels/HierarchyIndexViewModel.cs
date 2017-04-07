using DeanAndSons.Models.WAP;
using System.Collections.Generic;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class HierarchyIndexViewModel
    {
        public string StaffID { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public Staff Superior { get; set; }

        public ICollection<Staff> Subordinates { get; set; }

        public ImageAppUser Image { get; set; }

        public HierarchyIndexViewModel(Staff staff)
        {
            StaffID = staff.Id;
            Forename = staff.Forename;
            Surname = staff.Surname;
            Image = staff.getImage(staff.Image);

            if (staff.Superior == null)
            {
                Superior = new Staff();
                Superior.Forename = "Dean & ";
                Superior.Surname = "Sons";
            }
            else
            {
                Superior = staff.Superior;
            }

            Subordinates = staff.Subordinates;
        }
    }
}