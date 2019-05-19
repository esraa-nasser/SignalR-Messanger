using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MessangerChat.Models.Entity
{
    public class Message
    {
        [Display(Name = "Message ID ")]
        public int messageId { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Read")]
        [Range(0, 2)]
        public int read { get; set; }

        [InverseProperty("messagesSend")]
        public virtual ApplicationUser Sender { get; set; }
        [InverseProperty("messagesRecieve")]
        public virtual ApplicationUser Reciever { get; set; }
    }
}