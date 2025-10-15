using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class ChatRoomType
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? MaxMember { get; set; }

    public bool? Istemp { get; set; }

    public bool? IsChannel { get; set; }

    public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
}
