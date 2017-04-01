using System.Collections.Generic;

namespace DeanAndSons.Models.IMS.ViewModels
{
    public class StaffManagerIndexIMSViewModel
    {
        public List<Staff> Directors { get; set; }

        public List<Staff> Managers { get; set; }

        public List<Staff> Agents { get; set; }

        /// <summary>
        /// Create view model with set lists
        /// </summary>
        /// <param name="directors">List of directors</param>
        /// <param name="managers">List of managers</param>
        /// <param name="agents">List of agents</param>
        public StaffManagerIndexIMSViewModel(List<Staff> directors, List<Staff> managers, List<Staff> agents)
        {
            Directors = directors;
            Managers = managers;
            Agents = agents;
        }
    }
}