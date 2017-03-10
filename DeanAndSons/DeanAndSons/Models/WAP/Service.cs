using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanAndSons.Models.WAP
{
    public class Service
    {
        public int ServiceID { get; set; }

        [Required]
        [MaxLength(75), MinLength(5)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000), MinLength(5)]
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Required]
        public bool Deleted { get; set; } = false;

        [Required]
        public string StaffOwnerID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("StaffOwnerID")]
        public Staff StaffOwner { get; set; }

        public ICollection<ImageService> Images { get; set; } = new Collection<ImageService>();

        public string imgLocation = "/Storage/Services";

        public Service()
        {

        }

        //Takes a collectin of uploaded images and creates a new ImpageProperty for each non null && non empty entry
        private ICollection<ImageService> addImages(ICollection<HttpPostedFileBase> files)
        {
            var images = new Collection<ImageService>();
            ImageType imgType = ImageType.ServiceHeader;

            for (int i = 0; i < files.Count; i++)
            {
                if (files.ElementAt(i) != null && files.ElementAt(i).ContentLength != 0)
                {
                    if (i != 0)
                        imgType = ImageType.ServiceBody;

                    images.Add(new ImageService(files.ElementAt(i), imgType, imgLocation, this));
                }
            }

            return images;
        }
    }
}