using System.Threading.Channels;
using Dashboard.Blazor.Data;
using Dashboard.Blazor.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Dashboard.Blazor.Hubs;

public sealed class ChatStreamHub : Hub
{
    private readonly ChatService _chatService;

    public ChatStreamHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public ChannelReader<ChatMessage> GetChannelStream(CancellationToken cancellationToken)
    {
        return _chatService.ChatObservable.AsChannelReader();
    }

    public Task SendMessage(string room, string message)
    {
        _chatService.Send(room, message);
        return Task.CompletedTask;
    }
}
