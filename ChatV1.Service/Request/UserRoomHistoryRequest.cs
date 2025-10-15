using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class UserRoomHistoryRequest
    {
        public string UserName {  get; set; }
        public string ChatRoomName {  get; set; }

        public Guid? Chatguid { get; set; }
    }

    public class UserRoomHistoryRequestV2
    {
        public string UserName { get; set; }
        public string ChatRoomName { get; set; }

        public long TimeStamp { get; set; }
    }


    public class UserRoomHistoryRequestV3
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserName { get; set; } = "";
        public string ChatRoomName { get; set; }

        public Guid? Chatguid { get; set; }

        public bool isForward {  get; set; }
        public int perPage {  get; set; }
        public bool isIncluded {  get; set; }
    }
}
