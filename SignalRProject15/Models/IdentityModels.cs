using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MessangerChat.Models.Entity;

namespace MessangerChat.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ICollection<OnlineUser> onlineUsers { get; set; }
        public ICollection<Message> messagesSend { get; set; }
        public ICollection<Message> messagesRecieve { get; set; }
        public ICollection<MessageInGroup> messagesIngroup { get; set; }
        public ICollection<Group> Groups { get; set; }

        public ApplicationUser()
        {
            onlineUsers = new HashSet<OnlineUser>();
            messagesSend = new HashSet<Message>();
            messagesRecieve = new HashSet<Message>();
            messagesIngroup = new HashSet<MessageInGroup>();
            Groups = new HashSet<Group>();
        }

        public string Image { get; set; }
        
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string first_name { get; set; }

       

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string last_name { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageInGroup> MessageInGroups { get; set; }
        public virtual DbSet<OnlineUser> OnlineUsers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }
}
