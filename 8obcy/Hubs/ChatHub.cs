using Microsoft.AspNetCore.SignalR;

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
        public async Task GetConnectionId()
        {
            var groupName = _groupManager.ChangeGroup(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GetConnectionId",Context.ConnectionId);
        }
        public async Task ChangeGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _groupManager.GetCurrentGroup(Context.ConnectionId));
            await Groups.AddToGroupAsync(Context.ConnectionId, _groupManager.GetCurrentGroup(Context.ConnectionId));
        }

    }
}
