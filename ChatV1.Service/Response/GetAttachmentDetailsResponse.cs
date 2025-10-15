using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetAttachmentDetailsResponse:ResponseMessage
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContent { get; set; }
        public string FileType { get; set; }

    }

    public class GetAttachmentDetailsResponseFile
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }

    }



}
