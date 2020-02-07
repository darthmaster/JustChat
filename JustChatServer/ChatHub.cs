using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JustChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string username, string message) => await Clients.All.SendAsync("Receive", username, message, DateTime.UtcNow).ConfigureAwait(false);
        public override async Task OnConnectedAsync()
        {
            Console.Write($"[{DateTime.Now.TimeOfDay}] {Context.ConnectionId}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" is connected.");
            Console.ResetColor();
            await Clients.All.SendAsync("Receive", string.Empty, $"{Context.ConnectionId} врывается в тред!").ConfigureAwait(false);
            await base.OnConnectedAsync().ConfigureAwait(false);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.Write($"[{DateTime.Now.TimeOfDay}] {Context.ConnectionId}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" is disconnected.");
            Console.ResetColor();
            await Clients.All.SendAsync("Receive", string.Empty, $"{Context.ConnectionId} покинул нас. Press F to pay respect.").ConfigureAwait(false);
            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }
    }
}
