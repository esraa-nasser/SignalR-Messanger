using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MessangerChat.Models.Entity
{
    public class Group
    {
        [Display(Name = "Group ID ")]
        public int groupId { get; set; }
        [Display(Name = "Group name ")]
        public string group_name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public Group()
        {
            Users = new HashSet<ApplicationUser>();
        }
    }
}