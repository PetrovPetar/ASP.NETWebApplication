using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Models
{

    public class ApplicationUser : IdentityUser
    {
       public ApplicationUser()
        {
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
        }
        [StringLength(90)]
        [Required]
        public string FullName { get; set; }

        public string AboutMe { get; set; }
        public string ProfilePic { get; set; }
        public string Role { get; set; }
        public virtual HashSet<Post> Posts { get; set; }
        public virtual HashSet<Comment> Comments { get; set; }

        public virtual HashSet<Image> Images { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}