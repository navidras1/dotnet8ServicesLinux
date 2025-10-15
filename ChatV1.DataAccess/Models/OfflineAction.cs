using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Models
{
    public class OfflineAction
    {
        public long Id { get; set; }
        public string FromUseName { get; set; }
        public string ToUserName { get; set; }
        public long ActionTypeId { get; set; }
        public string Action { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool Done { get; set; }
        public DateTime? DoneDateTime { get; set; }
        public virtual ActionType ActionType { get; set; }

    }
}
