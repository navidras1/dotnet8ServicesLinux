using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class CheckIfRoomExistsResponse: ResponseMessage
    {
        public bool RoomFound { get; set; }= true;
        public long? RoomId { get; set; } = null;
    }
}
