namespace DeanAndSons.Models.JSONClasses
{
    public class SaveSearchJSON
    {
        public string Location { get; set; }
        public int Radius { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int Beds { get; set; }
        public PropertyAge Age { get; set; }
        public int CategorySort { get; set; }
        public int OrderSort { get; set; }
    }
}