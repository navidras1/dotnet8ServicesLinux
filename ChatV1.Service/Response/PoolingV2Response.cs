using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class PoolingV2Response : ResponseMessage
    {
        public List<PoolingV2ResponseDetails> result {  get; set; }=new List<PoolingV2ResponseDetails>();
    }

    public class PoolingV2ResponseDetails
    {
        public PoolingV2Room room { get; set; } 
        public PoolingV2Message message { get; set; } 

    }

    public class PoolingV2Message
    {
        public int total { get; set; }
        public List<PoolingV2MessageDetail> list { get; set; }
    }

    public class PoolingV2MessageDetail
    {
        public string fromFullName { get; set; }
        public string fromUserName { get; set; }
        public int fromUserRemoteId { get; set; }
        public Guid guid { get; set; }
        public long date { get; set; }
        public string status { get; set; }
        public bool? isRtl { get; set; }
        public string? forwardedBy { get; set; }
        public PoolingV2Attachment? attachment { get; set; }
        public string? replyOf { get; set; }
        public string message { get; set; }
    }

    public class PoolingV2Attachment
    {
        public long? id { get; set; }
        public string name { get; set; }
        public long? size { get; set; }
        public string type { get; set; }
        public string contentType { get; set; }
    }

    public class PoolingV2Room
    {
        public string type { get; set; }
        public string username { get; set; }
        public string fullName { get; set; }
        public string role { get; set; }
        public int? remoteId { get; set; }
        public int unreadCount { get; set; }
    }

}
