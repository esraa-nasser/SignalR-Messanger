using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using MessangerChat.Models;
using MessangerChat.Models.Entity;
using Message = MessangerChat.Models.Entity.Message;

namespace MessangerChat
{
    public class messengerHub : Hub
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public override Task OnConnected()
        {
            var userID = Context.User.Identity.GetUserId();
            var user = dbContext.Users.FirstOrDefault(ww => ww.Id == userID);
            OnlineUser onlineOne = new OnlineUser()
            {
                User = user,
                ConnectionId = Context.ConnectionId
            };
            dbContext.OnlineUsers.Add(onlineOne);
            dbContext.SaveChanges();
            var userGroups = (from u in dbContext.Users
                              from g in u.Groups
                              join ug in dbContext.Groups on g.groupId equals ug.groupId
                              where u.Id == userID
                              select ug.groupId).ToList();
            var all = (from p in dbContext.Groups where userGroups.Contains(p.groupId) select p).ToList();
            if (all.Count() > 0)
            {
                foreach (var item in all)
                {
                    Groups.Add(Context.ConnectionId, item.group_name);
                }
            }
            var recievedMsg = (from p in dbContext.Messages where p.Reciever.Id == userID select p).ToList();
            foreach (var item in recievedMsg)
            {
                if (item.read == 0)
                {
                    item.read = 1;
                }
            }
            //var recievedMsginGrp = (from p in dbContext.MessageInGroups where p..Id == userID select p).ToList();
            //foreach (var item in recievedMsginGrp)
            //{
            //    if (item.read == 0)
            //    {
            //        item.read = 1;
            //    }
            //}
            dbContext.SaveChanges();
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = dbContext.OnlineUsers.FirstOrDefault(ww => ww.ConnectionId == Context.ConnectionId);
            if (user.User.Id != null)
            {
                dbContext.OnlineUsers.Remove(user);
                dbContext.SaveChanges();
            }        
            return base.OnDisconnected(stopCalled);
        }

        public void senddata(string message, string recieverId)
        {
            var senderId = Context.User.Identity.GetUserId();
            var result = (from p in dbContext.OnlineUsers
                          where p.User.Id == senderId || p.User.Id == recieverId
                          select p.ConnectionId).ToList();
            var userName = dbContext.Users.Where(ww => ww.Id == senderId).FirstOrDefault();
            Clients.Clients(result).newdata(userName.UserName,userName.Image, message);
            var user = dbContext.Users.FirstOrDefault(ww => ww.Id == senderId);
            var recieverr = dbContext.Users.FirstOrDefault(ww => ww.Id == recieverId);
            DateTime Date = DateTime.Now;
            string MsgDate = Date.ToString("G");
            var recieverconId = dbContext.OnlineUsers.FirstOrDefault(ww => ww.User.Id == recieverId);
            Message newmessag = new Message();
            if (recieverconId!=null)
            {       
                    newmessag.Date = MsgDate;
                    newmessag.Sender = user;
                    newmessag.Reciever = recieverr;
                    newmessag.Content = message;
                    newmessag.read = 1;
                    dbContext.Messages.Add(newmessag);
            }
            else
            {
                newmessag.Date = MsgDate;
                newmessag.Sender = user;
                newmessag.Reciever = recieverr;
                newmessag.Content = message;
                newmessag.read = 0;
                dbContext.Messages.Add(newmessag);
            }
            dbContext.SaveChanges();
        }
        public void joinGroup(string groupName)
        {
            var group = dbContext.Groups.FirstOrDefault(ww => ww.group_name == groupName);
            var userId = Context.User.Identity.GetUserId();
            var user = dbContext.Users.FirstOrDefault(ww => ww.Id == userId);
            if (group != null)
            {
                Clients.OthersInGroup(groupName).newMember(user.UserName, groupName);
                var groupId = group.groupId;
                group.Users.Add(user);
                dbContext.SaveChanges();
            }
            else
            {
                Groups.Add(Context.ConnectionId, groupName);
                Clients.OthersInGroup(groupName).newMember(user.UserName, groupName);
                Group g = new Group()
                {
                    group_name = groupName
                };
                dbContext.Groups.Add(g);
                user.Groups.Add(g);
                g.Users.Add(user);
                dbContext.SaveChanges();
            }
        }
        public void sendToGroup(string message, int groupId)
        {
            var senderId = Context.User.Identity.GetUserId();
            var gName = dbContext.Groups.FirstOrDefault(ww => ww.groupId == groupId);
            var userName = dbContext.Users.FirstOrDefault(ww => ww.Id == senderId);
            Clients.Group(gName.group_name).msgToGroup(userName.UserName,userName.Image, message);
            DateTime Date = DateTime.Now;
            string MsgDate = Date.ToString("G");
            MessageInGroup msgInGroup = new MessageInGroup()
            {
                Date = MsgDate,
                Group = gName,
                GroupMessageContent = message,
                User = userName
            };
            dbContext.MessageInGroups.Add(msgInGroup);
            dbContext.SaveChanges();
        }
        public void log()
        {
            Clients.All.lognew();

        }
    }
  
}