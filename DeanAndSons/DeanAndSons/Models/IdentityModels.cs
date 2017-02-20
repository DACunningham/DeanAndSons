using DeanAndSons.Models.WAP;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeanAndSons.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "User Name to Display")]
        public string UserNameDisp { get; set; }

        [Display(Name = "Lower Budget")]
        public int? BudgetLower { get; set; }

        [Display(Name = "Upper Budget")]
        public int? BudgetHigher { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [ForeignKey("Superior")]
        public string SuperiorID { get; set; }

        //Only want to allow one of these programmatically
        public ICollection<ContactUser> Contact { get; set; }

        public ICollection<ImageUser> Image { get; set; }

        [InverseProperty("Buyer")]
        public ICollection<Property> PropertysBuy { get; set; }

        [InverseProperty("Seller")]
        public ICollection<Property> PropertysSell { get; set; }

        public ApplicationUser Superior { get; set; }

        public ICollection<ApplicationUser> Subordinates { get; set; }

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

        public ApplicationDbContext()
            : base("LocalDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public enum UserType
    {
        Buyer = 1,
        Seller,
        BuyerSeller,
        Developer,
        Business,
        Staff
    }
}