using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage( string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
            

            Console.WriteLine(message);
        }
        public async Task ReceiveMess(string senderId,string groupId,string message)
        {

            await Clients.All.SendAsync("");
            Console.WriteLine(message);
        }
    }
}