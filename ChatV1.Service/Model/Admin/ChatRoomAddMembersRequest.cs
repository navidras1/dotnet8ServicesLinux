using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class ChatRoomAddMembersRequest
    {
        public string ChatRoomName {  get; set; }
        public List<ChatRoomMemberModel> ChatRoomMembers { get; set; } = new List<ChatRoomMemberModel>();
    }

    public class ChatRoomMemberModel
    {
        public string UserName {  get; set; }
        public bool IsAdmin { get; set; }
    }
}
