using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace _8obcy.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGroupManager _groupManager;
        public ChatHub(IGroupManager groupManager)
        {
            _groupManager = groupManager;
        }
        public async Task SendMessage(string username, string message)
        {
            await Clients.Group(_groupManager.GetCurrentGroup(Context.ConnectionId))
                .SendAsync("ReceiveMessage",username, message, this.Context.ConnectionId);
        }
        public async Task ChangeGroup(string message)
        {
            var currentGroupId = _groupManager.GetCurrentGroup(Context.ConnectionId);
            if (currentGroupId != String.Empty)
            {
                await SendEndConversationMessage(message);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, _groupManager.GetCurrentGroup(Context.ConnectionId));
                _groupManager.RemoveGroup(Context.ConnectionId);
            }
            _groupManager.ChangeGroup(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, _groupManager.GetCurrentGroup(Context.ConnectionId));
            
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await SendEndConversationMessage("Twój rozmówca się rozłączył");
            _groupManager.RemoveGroup(Context.ConnectionId);
        }
        public override async Task OnConnectedAsync()
        {
            var groupName = _groupManager.ChangeGroup(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GetConnectionId", Context.ConnectionId);
        }

        private async Task SendEndConversationMessage(string message)
        {
            await Clients.Group(_groupManager.GetCurrentGroup(Context.ConnectionId))
                .SendAsync("EndConversationMessage", message);
        }
    }
}
