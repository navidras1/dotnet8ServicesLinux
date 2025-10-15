using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ChatV1.DataAccess.Context
{
    public class MyModuleInitializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
