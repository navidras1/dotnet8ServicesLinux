using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class UserContanct
{
    public long Id { get; set; }

    public long? EmpMasterId { get; set; }

    public string? UserName { get; set; }

    public virtual EmpMaster? EmpMaster { get; set; }
}
