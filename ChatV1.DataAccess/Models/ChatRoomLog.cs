using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class ChatRoomLog
{
    public long Id { get; set; }

    public long ChatLogId { get; set; }

    public long ChatRoomId { get; set; }

    public virtual ChatLog ChatLog { get; set; } = null!;

    public virtual ChatRoom ChatRoom { get; set; } = null!;
}
