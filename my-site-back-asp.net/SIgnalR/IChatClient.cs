namespace mysite_back_asp.net.SIgnalR;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}
