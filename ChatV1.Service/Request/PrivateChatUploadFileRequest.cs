using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class PrivateChatUploadFileRequest
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string ContentType {  get; set; }
        public string? MinioBucket { get; set; } = null;
        //public string FromUserName { get; set; }
        //public string ToUserName { get; set; }
        //public string Message { get; set; }
    }
}
