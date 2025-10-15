using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class ChatRoom
{
    public long Id { get; set; }

    public string? ChatRoomName { get; set; }

    public bool? IsActive { get; set; }

    public string? Description { get; set; }

    public long? ChatRoomTypeId { get; set; }

    public string? CreatorUserName { get; set; }

    public DateTime? CreateDatetime { get; set; }

    public virtual ICollection<ChatRoomLog> ChatRoomLogs { get; set; } = new List<ChatRoomLog>();

    public virtual ICollection<ChatRoomMemeber> ChatRoomMemebers { get; set; } = new List<ChatRoomMemeber>();

    public virtual ChatRoomType? ChatRoomType { get; set; }
    public virtual long? ChatRoomLogoId { get; set; }
    public virtual bool? CanGetPushNotification { get; set; }
}
