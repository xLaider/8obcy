using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

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
            var currentGroup = _groupManager.GetCurrentGroup(Context.ConnectionId);
            if (currentGroup != null)
            {
                await Clients.Group(currentGroup.Id.ToString())
                .SendAsync("ReceiveMessage", username, message, this.Context.ConnectionId);
            }

        }
        public async Task ChangeGroup(string message, string username)
        {
            var currentGroup = _groupManager.GetCurrentGroup(Context.ConnectionId);
            if (currentGroup != null)
            {
                await SendEndConversationMessage(message, username);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroup.Id);
                _groupManager.RemoveGroupByConnectionId(Context.ConnectionId);
            }
            await HandleLookingForGroup();
            await Groups.AddToGroupAsync(Context.ConnectionId, _groupManager.GetCurrentGroup(Context.ConnectionId).Id);

        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await SendEndConversationMessage("Twój rozmówca się rozłączył", "");
            _groupManager.RemoveGroupByConnectionId(Context.ConnectionId);
        }
        public override async Task OnConnectedAsync()
        {
            await HandleLookingForGroup();
            await Clients.Caller.SendAsync("GetConnectionId", Context.ConnectionId);
        }
        private async Task HandleLookingForGroup()
        {
            await SendSearchingForConversationMessage();
            var group = _groupManager.ChangeGroup(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id);
            if (!group.Open)
            {
                await SendFoundConversationMessage();
            }
        }
        private async Task SendEndConversationMessage(string message, string username)
        {
            await Clients.Group(_groupManager.GetCurrentGroup(Context.ConnectionId).Id)
                .SendAsync("EndConversationMessage", message, username);
        }
        private async Task SendSearchingForConversationMessage()
        {
            await Clients.Caller.SendAsync("SearchingForConversationMessage", "Szukanie nieznajomego..."); 
        }
        private async Task SendFoundConversationMessage()
        {
            await Clients.Group(_groupManager.GetCurrentGroup(Context.ConnectionId).Id)
                .SendAsync("FoundConversationMessage", "Znaleziono nieznajomego!");
        }
    }
}
