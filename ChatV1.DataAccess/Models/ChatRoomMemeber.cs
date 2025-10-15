using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class ChatRoomMemeber
{
    public long Id { get; set; }

    public long ChatRoomId { get; set; }

    public int? UserId { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime CreateDateTime { get; set; }

    public bool IsActive { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? CantGetNotif {  get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;
}
