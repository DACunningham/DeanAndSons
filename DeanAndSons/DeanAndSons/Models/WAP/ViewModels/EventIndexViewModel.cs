using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DeanAndSons.Models.WAP.ViewModels
{
    public class EventIndexViewModel
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} field must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Description { get; set; }

        public ContactEvent Contact { get; set; }

        public ImageEvent Image { get; set; }

        public DateTime Created { get; set; }

        public EventIndexViewModel(Event e)
        {
            EventID = e.EventID;
            Title = e.Title;
            Description = createDescription(e.Description);
            Contact = e.getContact(e.Contact);
            Image = getImage(e);
            Created = e.Created;
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
                return desc.Substring(0, 200) + "...</div>";
            }
            else
            {
                return desc;
            }
        }

        //Checks if event's image value is null
        private ImageEvent getImage(Event item)
        {
            ImageEvent image = null;
            try
            {
                image = item.Images.First(i => i.Type == ImageType.EventHeader);
            }
            catch (InvalidOperationException)
            {
                image = new ImageEvent();
                image.Location = ImageEvent.defaultImgLocation;
            }

            return image;
        }
    }
}