using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetContactListRequest
    {
        public string OwnerUserName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
