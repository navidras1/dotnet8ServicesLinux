using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class ChatStatus
{
    public int Id { get; set; }

    public string ChatStatus1 { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ChatLog> ChatLogs { get; set; } = new List<ChatLog>();
    public virtual ICollection<UserChatRoomReciever> UserChatRoomRecievers { get; set; } = new List<UserChatRoomReciever>();
}
