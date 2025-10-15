using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class IsUserRoomAdminResponse :ResponseMessage
    {
        public bool IsAdmin { get; set; } = true;
    }
}
