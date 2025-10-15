using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class HistoryChatOfUsersV2Request
    {
        public string UserName {  get; set; }
        public List<HistoryChatRequestDetail> RquestDetails { get; set; }
    }

    public class HistoryChatRequestDetail
    {
        public string type { get; set; }
        public string name { get; set; }
        public Guid? lastMessageId { get; set; }
    }

    public class HistoryChatOfUsersPoolingRequest
    {
        public string UserName { get; set; }
        public List<HistoryChatRequestDetailPooling> RquestDetails { get; set; }
    }

    public class HistoryChatRequestDetailPooling
    {
        public string type { get; set; }
        public string name { get; set; }
        public long TimeStamp { get; set; }
        public string roomName {  get; set; }
    }

}
