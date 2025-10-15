using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Models
{
    public class ChatLogAttachment
    {
        public long Id { get; set; }
        public long ChatLogId { get; set; }
        public long ChatAttachmentId { get; set; }

        public ChatLog ChatLog { get; set; }
        public ChatAttachment ChatAttachment { get; set; }

    }
}
