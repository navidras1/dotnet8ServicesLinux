using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class ListOfRoomsWithUnreadMessagesRequestV2
    {
        public string userName { get; set; } = "";
        public int PageSize { get; set; }
        public int PageNumber {  get; set; }
    }

    public class ListOfRoomsWithUnreadMessagesRequestV3
    {
        public string userName { get; set; } = "";
        public long fromTimeStamp { get; set; }
        
    }


}
