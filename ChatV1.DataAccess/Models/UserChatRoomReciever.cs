using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Models
{
    public class UserChatRoomReciever
    {
        public long Id { get; set; }
        public string UserName { get; set; }   
        public int UserId { get; set; }
        public long ChatLogId {  get; set; }

        public long ChatroomId { get; set; }

        public int ChatStatusId { get; set; }

        public virtual ChatLog ChatLog { get; set; } = null!;
        public virtual ChatStatus ChatStatus { get; set; } = null!;

    }
}
