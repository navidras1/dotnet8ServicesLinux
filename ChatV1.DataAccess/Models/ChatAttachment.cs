using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Models
{
    public partial class ChatAttachment
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileAddress { get; set; }
        public string Description { get; set; }
        public long? FileSize { get; set; }
        public DateTimeOffset UploadDate { get; set; }
        public string? ContentType {  get; set; }
        public string? MinioBucket {  get; set; }
        public virtual ICollection<ChatLogAttachment> ChatLogAttachments { get; set; }
    }
}
