using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MessangerChat.Models.Entity
{
    public class OnlineUser
    {
        [Key]
        [Display(Name = "Connection ID ")]
        public string ConnectionId { get; set; }
        [Key]
        [ForeignKey("User")]
        public String Id;
        public virtual ApplicationUser User { get; set; }
    }
}