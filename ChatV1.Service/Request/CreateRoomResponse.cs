using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class CreateRoomResponse:ResponseMessage
    {
        public string ChatRoomName { get; set; }
        public long ChatRoomId { get; set; }
        public long? PicId { get; set; } = null;
        

    }
}
