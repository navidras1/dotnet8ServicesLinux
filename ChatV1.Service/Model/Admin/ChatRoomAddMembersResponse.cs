using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class ChatRoomAddMembersResponse:ResponseMessage
    {
        public List<AddMemberResult> AddMembers { get; set; }= new List<AddMemberResult>();
    }

    public class AddMemberResult
    {
        public long MemberId { get; set; }
        public string Name { get; set; }
    }
}
