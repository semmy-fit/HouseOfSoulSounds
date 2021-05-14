using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using HouseOfSoulSounds.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Helpers
{
    [Authorize]
    public class ChatHub:Hub
    {
        private const string prefix = "ИНФО: ";
        private readonly UserManager<User> userManager;
        private readonly DataManager dataManager;
        private readonly EFAppDbContext context;

        private readonly ConnectionDictionary<string> connections;

        public ChatHub(
            UserManager<User> userManager,
            DataManager dataManager,
            ConnectionDictionary<string> connections,
            EFAppDbContext _context)
        {
            this.userManager = userManager;
            this.dataManager = dataManager;
            this.connections = connections;
            this.context = _context;
        }

        public async Task SendMessage(string message,string recipient)
        {
            if(string.IsNullOrEmpty(message))
            {
                return;
            }
            var user = Context.User.Identity.Name;
            //User user1 = new User();
            //  var id = userManager.FindByIdAsync(user);

            if (recipient == default)
            {

                await Clients.All.SendAsync("ReceiveMessage",
                    $"{user}: " + message, recipient);

                var user1 = await userManager.FindByNameAsync(user);
                var mess = new Message { Text = message, UserId = user1.Id, User = user1, InstrumentItemId = new Guid(recipient), DateAdded = DateTime.Now };

                //var d = new Message { UserId=id.ToString(),User=user1,InstrumentItemId=instrument.Id,InstrumentItem=instrument,DateAdded=DateTime.Now,Text = message };
                // dataManager.Messages.SaveItem(mess);
                if (mess is not null)
                {
                    dataManager.Messages.SaveItem(mess);
                    //context.Messages.Add(mess);
                    //context.SaveChanges();
                }
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage",
                    $"{user}: " + message, recipient);
                var user1 = await userManager.FindByNameAsync(user);
                var mess = new Message { Text = message, UserId = user1.Id, User = user1, InstrumentItemId = new Guid(recipient), DateAdded = DateTime.Now };

                dataManager.Messages.SaveItem(mess);
                var recipientUser = await userManager.FindByNameAsync(recipient);
               
            }
        }

        public override async Task OnConnectedAsync()
        {
            connections.Add(Context.User.Identity.Name, Context.ConnectionId);
            if (connections.GetConnections(Context.User.Identity.Name).Count() == 1)
                await Clients.All.SendAsync("Notify",
                    $"{prefix}{Context.User.Identity.Name} вошел в чат");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            connections.Remove(Context.User.Identity.Name, Context.ConnectionId);
            if (!connections.GetConnections(Context.User.Identity.Name).Any())
                await Clients.All.SendAsync("Notify",
                    $"{prefix}{Context.User.Identity.Name} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}

