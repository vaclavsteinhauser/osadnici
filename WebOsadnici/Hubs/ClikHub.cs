using Microsoft.AspNetCore.SignalR;

namespace WebOsadnici.Hubs
{
    public class ClickHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}