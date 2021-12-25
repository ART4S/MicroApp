using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Ordering.SignalR;

[Authorize]
public class NotificationsHub : Hub
{
    private string GetUserId() => 
        Context.User!.Claims.Single(x => x.Type == "sub").Value;

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetUserId());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetUserId());
        await base.OnDisconnectedAsync(ex);
    }
}
