using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models;
using HouseOfSoulSounds.Helpers;
using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfSoulSounds.Controllers
{
    public class InstrumentsController : Controller
    {
        public DataManager _dataManager { get; set; }
        public EFAppDbContext _context { get; set; }
        public UserManager<User> _userManager { get; set; }
   
        public IQueryable<IdentityUser> users;
        public InstrumentsController(DataManager dataManager,EFAppDbContext context, UserManager<User> userManager)
        {
            this._dataManager = dataManager;
            this._context = context;
            this._userManager = userManager;
        }
        public async Task<IActionResult> Index(Guid id)
        {

           // var userid = users.Select(x => x.Id);
           // var username = users.Select(x => x.UserName);

            if (id != default)
            {
                var model = _dataManager.Instruments.GetItemById(id);
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.Chat = new ChatModel();
                model.Chat.UserName = User.Identity.Name;
                model.Chat.UpTo = DateTime.Now;
                model.Chat.IsBlocked=user.Blocked??true;
                model.Chat.IsModerator = User.IsInRole(Config.RoleModer);
                model.Chat.Email = user.Email;
                
                return View("Show", model);
            }

            return View();
        }
        public IActionResult AddMessage()
        {
   
            //if (message1 is not null)
            //{
            //    foreach (var item in message1)
            //    {
            //        item.User.ToString();
            //        item.Text.ToString();
            //        item.DateAdded.ToString();
            //    }
            //}
            //var data = new Message { 
            //Text=message.Text,
            //User=message.User,
            //UserId=message.UserId,
            //DateAdded=message.DateAdded
            //};
            //Message data = new Message();
            //IQueryable<Message> message1 = from m in _dataManager.Messages.Items
            //                               select new Message { Id=id,UserId = user.Id, Text =data.Text, User =data.User};
            ////  data.User= (User)user;
            //data.Id = message1.Select(x => x.Id).FirstOrDefault();
            //data.User = message1.Select(x => x.User).FirstOrDefault();
            //data.UserId = message1.Select(x => x.UserId).FirstOrDefault();
            //data.Text = message1.Select(x => x.Text).FirstOrDefault();
            //_dataManager.Messages.SaveItem(data);
          return  RedirectToAction("Chat","");
        
        }
        public IActionResult Welcome()
        {
            return View("gutar");
        }

    }
}
