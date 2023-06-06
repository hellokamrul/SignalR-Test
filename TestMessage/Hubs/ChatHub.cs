using Microsoft.AspNetCore.SignalR;
using TestMessage.Models;

namespace TestMessage.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message) =>
            await Clients.All.SendAsync("RecieveMessage",message);
    }
}
