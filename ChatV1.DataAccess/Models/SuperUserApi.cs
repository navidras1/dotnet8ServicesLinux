using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Models
{
    public class SuperUserApi
    {
        public long Id { get; set; }
        public string? ApiPath { get; set; }
        public string? ControllerName { get; set; }
    }
}
