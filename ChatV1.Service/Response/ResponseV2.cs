using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class ResponseV2
    {
        public string fromUserName { get; set; }
        //public int countOfUnreadMessage { get; set; }
        public string lastMessage { get; set; }
        public DateTimeOffset createDate { get; set; }
        public int pkEmployee { get; set; }
        public string fullName { get; set; }
        public bool? isRtl { get; set; }
        public string status {  get; set; }
    }

    public class HistoryMessage
    {
        public int Total { get; set; }
        public List<ResponseV2_V2> List{ get; set; }
    }

    public class ResponseV2_V2
    {
        public string fromUserName { get; set; }
        //public int countOfUnreadMessage { get; set; }
        public string toUserName { get; set; }
        public string lastMessage { get; set; }
        public long createDate { get; set; }
        public int pkEmployee { get; set; }
        public string fullName { get; set; }
        public bool? isRtl { get; set; }
        public string status {  get; set; }
        public Guid messageId {  get; set; }
        public GetFileDetailResponse attachment { get; set; } = null;
        public string chatRoomName { get; internal set; } = "private";
        public string chatRoomType { get; internal set; } = "private";
        public string forwardedBy { get; set; }
        public string replyOf {  get; set; }
        public string? msgFromFullName {  get; set; }
    }




}
