using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatV1.DataAccess.CommonModels;
using ChatV1.DataAccess.Repository;
namespace ChatV1.Service.Response
{
    public class PoolingResponse:ResponseMessage
    {
        public GetAllFromSPWithOutputResponseViewModel list {  get; set; }
        public HistoryChatOfUsersResponseV2 messages {  get; set; }

    }
}
