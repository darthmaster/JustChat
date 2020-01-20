using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JustChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string username, string message) => await Clients.All.SendAsync("Receive", username, message);
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Receive", string.Empty, $"{Context.ConnectionId} врывается в тред!");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Receive", string.Empty, $"{Context.ConnectionId} покинул нас. Press F to pay respect.");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
