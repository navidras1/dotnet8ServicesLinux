using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatV1.DataAccess.Models;
[Index(nameof(FromUserName), nameof(ToUserName), nameof(ChatGuid))]
public partial class ChatLog
{
    public long Id { get; set; }

    public int FromEmpId { get; set; }

    public int ToEmPid { get; set; }

    [Encrypted]
    public string Message { get; set; } = null!;

    public int ChatStatusId { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public Guid ChatGuid { get; set; }

    public string? FromUserName { get; set; }

    public string? ToUserName { get; set; }

    public virtual ICollection<ChatRoomLog> ChatRoomLogs { get; set; } = new List<ChatRoomLog>();

    public virtual ICollection<ChatLogAttachment> ChatAttachments { get; set; } = new List<ChatLogAttachment>();

    public virtual ICollection<UserChatRoomReciever> UserChatRoomReciever { get; set; } = new List<UserChatRoomReciever>();

    public virtual ChatStatus ChatStatus { get; set; } = null!;
    public bool? IsRtl { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTimeOffset? ClientDateTime { get; set; }
    public string? ForwardedBy {  get; set; }
    [Encrypted]
    public string? Reply { get; set; }
}
