using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Models
{
    public class ActionType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<OfflineAction> OfflineActions { get; set; }
    }
}
