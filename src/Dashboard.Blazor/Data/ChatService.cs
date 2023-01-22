using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Websocket.Client;

namespace Dashboard.Blazor.Data;

public sealed class ChatService : IDisposable
{
    private readonly ILogger<ChatService> _logger;
    private readonly IWebsocketClient _ws;

    public ChatService(ILogger<ChatService> logger, IOptions<TornConfiguration> options)
    {
        _logger = logger;

        var uid = options.Value.Uid;
        var secret = options.Value.Secret;
        var uri = new Uri($"wss://ws-chat.torn.com/chat/ws?uid={Uri.EscapeDataString(uid)}&secret={Uri.EscapeDataString(secret)}");
        _ws = new WebsocketClient(uri, () =>
        {
            var client = new ClientWebSocket();

            client.Options.SetRequestHeader("Host", "ws-chat.torn.com");
            client.Options.SetRequestHeader("Origin", "https://www.torn.com");

            return client;
        });

        _ws.ReconnectTimeout = null;
        _ws.ErrorReconnectTimeout = TimeSpan.FromSeconds(15);

        _ws.ReconnectionHappened.Subscribe(info =>
            _logger.LogInformation("Reconnection happened, type: {Type}", info.Type));

        _ws.DisconnectionHappened.Subscribe(info =>
            _logger.LogInformation("Disconnection happened, type: {Type}", info.Type));

        _ = _ws.Start();
    }

    public IObservable<ChatMessage> ChatObservable =>
        _ws.MessageReceived
            .Select(response =>
            {
                var msg = JsonSerializer.Deserialize<Result>(response.Text, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Encoder = JavaScriptEncoder.Default,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                })!;

                var chatMessage = new ChatMessage(
                    msg.Data[0].MessageId,
                    msg.Data[0].MessageText,
                    msg.Data[0].RoomId,
                    msg.Data[0].SenderId,
                    msg.Data[0].SenderIsStaff,
                    msg.Data[0].SenderName,
                    DateTimeOffset.FromUnixTimeSeconds(msg.Data[0].Time));

                _logger.LogInformation("Message received: {@Message}", chatMessage);

                return chatMessage;
            });

    public void Dispose()
    {
        _ws.Dispose();
    }

    public void Send(string channel, string message)
    {
        _ws.Send($$"""{"proc":"rooms/sendMessage","data":{"roomId":["{{channel}}"],"messageText":["{{message}}"]},"v":4}""");
    }
}
