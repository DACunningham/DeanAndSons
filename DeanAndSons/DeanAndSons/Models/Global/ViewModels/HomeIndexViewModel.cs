using System;
using System.Collections.Generic;
using System.Linq;

namespace DeanAndSons.Models.Global.ViewModels
{
    public class HomeIndexViewModel
    {
        public ICollection<HomeIndexProperty> Properties { get; set; }

        public HomeIndexViewModel(IEnumerable<Property> props)
        {
            Properties = new List<HomeIndexProperty>();

            foreach (var item in props)
            {
                var _imgLoc = getImage(item).Location;
                var _tmp = new HomeIndexProperty(item.PropertyID, item.Title, _imgLoc);

                Properties.Add(_tmp);
            }
        }

        //Checks if property's image value is null
        private ImageProperty getImage(Property item)
        {
            ImageProperty image = null;
            try
            {
                image = item.Images.First(i => i.Type == ImageType.PropertyHeader);
            }
            catch (InvalidOperationException)
            {
                image = new ImageProperty();
                image.Location = ImageProperty.defaultImgLocation;
            }

            return image;
        }
    }

    public class HomeIndexProperty
    {
        public int PropertyID { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }

        public HomeIndexProperty(int id, string title, string imgLoc)
        {
            PropertyID = id;
            Title = title;
            ImageLocation = imgLoc;
        }
    }
}