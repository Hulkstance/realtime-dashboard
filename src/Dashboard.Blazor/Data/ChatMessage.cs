namespace Dashboard.Blazor.Data;

public record ChatMessage(
    string MessageId,
    string MessageText,
    string RoomId,
    string SenderId,
    bool SenderIsStaff,
    string SenderName,
    DateTimeOffset Time);
