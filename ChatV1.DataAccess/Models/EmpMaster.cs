using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class EmpMaster
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public DateTime? LastSeenDate { get; set; }

    public bool? IsSuperUser { get; set; }

    public virtual ICollection<UserContanct> UserContancts { get; set; } = new List<UserContanct>();
}
