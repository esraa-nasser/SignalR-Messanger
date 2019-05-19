using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MessangerChat.Models.Entity
{
    public class MessageInGroup
    {
        [Display(Name = "Group_Message_ID ")]
        public int MessageInGroupId { get; set; }

        [Display(Name = "GroupMessageContent")]
        public string GroupMessageContent { get; set; }

        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Read")]
        [Range(0,2)]
        public int read { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Group Group { get; set; }
    }
}