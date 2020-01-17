using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JustChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string username) 
        {
            await Clients.All.SendAsync("Receive", message, username);
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} врывается в тред!");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} покинул нас. Press F to pay respect.");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
