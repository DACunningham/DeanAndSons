using DeanAndSons.Models.WAP;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeanAndSons.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // User's first name
        [Required]
        [StringLength(60, ErrorMessage = "Your {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        // User's second name
        [Required]
        [StringLength(60, ErrorMessage = "Your {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        // Information about this user
        public string About { get; set; }

        // The user name that should be displayed around the site (not used for validation)
        [Required]
        [Display(Name = "User Name to Display")]
        public string UserNameDisp { get; set; }

        //Is this user deleted? This allows logic deletes so, admins can administrate and undelete items.
        [Required]
        public bool Deleted { get; set; }

        // List of one contact details for this user (some reason EF won't let me have 1-1 relationship here)
        public ICollection<ContactUser> Contact { get; set; }

        // List of images associated with this user
        public ICollection<ImageAppUser> Image { get; set; }

        //Checks if property's contact value is null
        public ContactUser getContact(ICollection<ContactUser> contactCol)
        {
            ContactUser contact = null;
            try
            {
                contact = contactCol.First();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException)
            {
                contact = new ContactUser();
                contact.PropertyNo = "No address found";
                contact.Town = "No address found";
            }

            return contact;
        }

        //Checks if property's image value is null
        public ImageAppUser getImage(ICollection<ImageAppUser> item)
        {
            ImageAppUser image = null;
            try
            {
                image = item.First(i => i.Type == ImageType.ProfileHeader);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException)
            {
                image = new ImageAppUser();
                image.Location = ImageAppUser.defaultImgLocation;
            }

            return image;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Property> Propertys { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Event> Events { get; set; }

        public ApplicationDbContext()
            : base("LocalDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}