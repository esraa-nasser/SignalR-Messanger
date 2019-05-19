using Microsoft.AspNet.Identity;
using MessangerChat.Models;
using MessangerChat.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessangerChat.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var user = dbContext.Users.FirstOrDefault(ww => ww.Id == id);
            var unreadmsg = (from p in dbContext.Messages where p.Reciever.Id == id && p.read!=2 select p).ToList();
            var all = dbContext.Users.ToList().Where(i => i.Id != User.Identity.GetUserId());
            ViewBag.users = all.Count();
            ViewBag.countMsg = unreadmsg.Count();
            var userGroups = (from u in dbContext.Users
                              from g in u.Groups
                              join ug in dbContext.Groups on g.groupId equals ug.groupId
                              where u.Id == id
                              select ug.groupId).ToList();
            var allgroup = (from p in dbContext.Groups where userGroups.Contains(p.groupId) select p).ToList();
            ViewBag.count = allgroup.Count();
            return View(user);
        }
        public ActionResult GetAllUsers()
        {
            var all = dbContext.Users.ToList().Where(i=>i.Id!=User.Identity.GetUserId());
            ViewBag.users = all.Count();
            return PartialView(all);
        }
        public ActionResult GetAllUsersWithChat()
        {
            var userId = User.Identity.GetUserId();
            var all = dbContext.Users.Where(i => i.Id != userId).ToList();
            ViewBag.users = all.Count();
            var on = dbContext.OnlineUsers.Where(i => i.User.Id != userId).Select(p => p.User.Id).ToList();
            ViewBag.on = on;
            return View(all);
        }
        public ActionResult Groups()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = dbContext.Users.FirstOrDefault(ww => ww.Id == userId);
            var userGroups = (from u in dbContext.Users
                              from g in u.Groups
                              join ug in dbContext.Groups on g.groupId equals ug.groupId
                              where u.Id == userId
                              select ug.groupId).ToList();
            var all = (from p in dbContext.Groups where userGroups.Contains(p.groupId) select p).ToList();
            ViewBag.count = all.Count();
            return PartialView(all);
        }

        [HttpGet]
        public ActionResult Startchat(string id)
        {
            var reciever = dbContext.Users.FirstOrDefault(ww => ww.Id == id);
            var sender = User.Identity.GetUserId();
            var message = (from msg in dbContext.Messages
                           where ((msg.Sender.Id == sender && msg.Reciever.Id == reciever.Id) || (msg.Sender.Id == reciever.Id && msg.Reciever.Id == sender))
                           select msg).ToList();
            foreach (var item in message)
            {
                if (item.Reciever.Id == sender)
                {
                    item.read = 2;
                }

            }         
            dbContext.SaveChanges();
            var on = (from o in dbContext.OnlineUsers where o.User.Id == reciever.Id select o).FirstOrDefault();
            ViewBag.isOn = on;
            ViewBag.recieverId = reciever.Id;
            ViewBag.recieverName = reciever.UserName;
            ViewBag.img = reciever.Image;
            ViewBag.user = dbContext.Users.Where(ww => ww.Id == sender).Select(p => p.Image).FirstOrDefault();
            return PartialView(message);
        }

        [HttpPost]
        public ActionResult Startchat(Message msg)
        {
            var recieverId = msg.Reciever.Id;
            dbContext.Messages.Add(msg);
            dbContext.SaveChanges();
            return RedirectToAction("Startchat", recieverId);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}