using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class FileUploadPrivateChatRequest
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string Message { get; set; }
    }
}
