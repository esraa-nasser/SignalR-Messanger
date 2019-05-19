using Microsoft.AspNet.Identity;
using MessangerChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessangerChat.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {  
            return View();
        }
        public ActionResult getAllGroups()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = dbContext.Users.FirstOrDefault(ww => ww.Id == userId);
            var userGroups = (from u in dbContext.Users
                              from g in u.Groups
                              join ug in dbContext.Groups on g.groupId equals ug.groupId
                              where u.Id==userId
                              select ug.groupId).ToList();
            var all = (from p in dbContext.Groups where userGroups.Contains(p.groupId) select p).ToList();
            ViewBag.count = all.Count();
            return PartialView(all);       
        }
        public ActionResult sendToGroup(int id)
        {
            var group = dbContext.Groups.FirstOrDefault(ww => ww.groupId == id);
            ViewBag.groupId = id;
            var userId = User.Identity.GetUserId();
            var msgInGroup = (from p in dbContext.MessageInGroups where p.Group.groupId == id orderby p.Date descending select p).ToList();
            var user = dbContext.Users.Where(i => i.Id == userId).Select(i => i.Image).FirstOrDefault();
            ViewBag.img = user;
            ViewBag.groupName = group.group_name;
            return View(msgInGroup);
        }
    }
}