using DeanAndSons.Models.JSONClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanAndSons.Models.WAP
{
    public class SavedSearch
    {
        public int ID { get; set; }

        public string Location { get; set; }

        public int Radius { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public int Beds { get; set; }

        public PropertyAge Age { get; set; }

        public int CategorySort { get; set; }

        public int OrderSort { get; set; }

        [ForeignKey("Customer")]
        public string CustomerID { get; set; }

        public Customer Customer { get; set; }

        public SavedSearch()
        {

        }

        public SavedSearch(SaveSearchJSON ssj, string userID)
        {
            Location = ssj.Location;
            Radius = ssj.Radius;
            MinPrice = ssj.MinPrice; MaxPrice = ssj.MaxPrice;
            Beds = ssj.Beds;
            Age = ssj.Age;
            CategorySort = ssj.CategorySort;
            OrderSort = ssj.OrderSort;
            CustomerID = userID;
        }
    }
}