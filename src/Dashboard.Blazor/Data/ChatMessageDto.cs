namespace Dashboard.Blazor.Data;

public class ChatMsg
{
    public string MessageId { get; set; }
    public string MessageText { get; set; }
    public string RoomId { get; set; }
    public string SenderId { get; set; }
    public bool SenderIsStaff { get; set; }
    public string SenderName { get; set; }
    public int SequenceNumber { get; set; }
    public int Time { get; set; }
    public string Type { get; set; }
}

public class Result
{
    public List<ChatMsg> Data { get; set; }
    public string Proc { get; set; }
    public int Status { get; set; }
}
