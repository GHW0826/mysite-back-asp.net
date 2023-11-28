using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace mysite_back_asp.net.SIgnalR.hub;

public class ChatHub : Hub
{
    /*
    
    ws://{url}/{channel_name} connect
    {"protocol":"json","version":1} send

    "target" -> HubMethodName Attribute
    {"type":1,"target":"SendMessageToUser","arguments":["X_YOnKS-ksYj5_MzY8-ssg","tes22t2"]}

    clientid != userid

    */

    // <HubMethods>
    public Task SendMessage(string user, string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public Task SendMessageToCaller(string user, string message)
    {
        return Clients.Caller.SendAsync("ReceiveMessage", user, message);
    }

    public Task SendMessageToGroup(string user, string message)
    {
        return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);
    }
    // </HubMethods>

    // <HubMethodName>
    [HubMethodName("SendMessageToUser")]
    public Task DirectMessage(string user, string message)
    {
       // var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Clients.Client(user).SendAsync("ReceiveMessage", message);
    }
    // </HubMethodName>

    // <ThrowHubException>
    public Task ThrowException()
    {
        throw new HubException("This error will be sent to the client!");
    }
    // </ThrowHubException>

    // <OnConnectedAsync>
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        await base.OnConnectedAsync();
        await this.Clients.Group("SignalR Users").SendAsync("Send", $"{this.Context.ConnectionId} joined \"SignalR Users\"");
        await SendMessage(Context.User.Identity.Name, "test2");
    }
    // </OnConnectedAsync>

    // <OnDisconnectedAsync>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", "I", "disconnect");
        await base.OnDisconnectedAsync(exception);
    }
    // </OnDisconnectedAsync>
}