using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Hubs
{
    public class MisGroupChatHub : Hub
    {
        public async Task JoinGroupChat(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Group_{groupId}");
        }

        public async Task LeaveGroupChat(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Group_{groupId}");
        }

        public async Task SendMessage(string groupId, object messageData)
        {
            await Clients.Group($"Group_{groupId}").SendAsync("ReceiveMessage", messageData);
        }

        public async Task BroadcastChatMessage(string groupId, string senderId, string senderName, string senderRole, string content, string createdAt)
        {
            await Clients.Group($"Group_{groupId}").SendAsync("ReceiveChatMessage", new
            {
                groupId,
                senderId,
                senderName,
                senderRole,
                content,
                createdAt
            });
        }
    }
}
