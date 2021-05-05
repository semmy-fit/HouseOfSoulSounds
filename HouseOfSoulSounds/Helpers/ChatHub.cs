using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using HouseOfSoulSounds.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HouseOfSoulSounds.Helpers
{
    [Authorize]
    public class ChatHub:Hub
    {
        private const string prefix = "ИНФО: ";
        private readonly UserManager<IdentityUser> userManager;
        private readonly DataManager dataManager;

        private readonly ConnectionDictionary<string> connections;

        public ChatHub(
            UserManager<IdentityUser> userManager,
            DataManager dataManager,
            ConnectionDictionary<string> connections)
        {
            this.userManager = userManager;
            this.dataManager = dataManager;
            this.connections = connections;
        }

        public async Task SendMessage(string message, string recipient)
        {
            var user = Context.User.Identity.Name;
            if (recipient == default)
            {
                await Clients.All.SendAsync("ReceiveMessage",
                    $"{user}: " + message);
                //ЗДЕСЬ МОЖНО ПОПИЛИТЬ СОХРАНЕНИЕ СООБЩЕНИЙ
            }
            else
            {
                var recipientUser = await userManager.FindByNameAsync(recipient);
                if (recipientUser is null)
                {
                    await Clients.Clients(connections.GetConnections(user).ToList())
                        .SendAsync("ReceiveMessage",
                        $"(приватно){prefix}участник \"{recipient}\" не найден");
                    return;
                }

                await Clients.Clients(connections.GetConnections(user).ToList())
                    .SendAsync("ReceiveMessage",
                        $"(приватно для {recipient}): " + message);
                await Clients.Clients(connections.GetConnections(recipient).ToList())
                    .SendAsync("ReceiveMessage",
                    $"(приватно от {user}): " + message);
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

