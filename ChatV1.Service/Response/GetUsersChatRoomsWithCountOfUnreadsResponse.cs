using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetUsersChatRoomsWithCountOfUnreadsResponse
    {
        public string chatroomname {  get; set; }
        public int countOfMembers { get;set; }
        public bool? isChannel {  get; set; }
        public string creator { get; set; }
        public bool? isAdmin { get; set; }
    }
}
