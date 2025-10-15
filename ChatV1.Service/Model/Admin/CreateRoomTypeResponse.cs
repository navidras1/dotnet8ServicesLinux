using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class CreateRoomTypeResponse : ResponseMessage
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}
