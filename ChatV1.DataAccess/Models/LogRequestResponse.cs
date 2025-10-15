using System;
using System.Collections.Generic;

namespace ChatV1.DataAccess.Models;

public partial class LogRequestResponse
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public string? ControllerName { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public DateTime? CreatedDate { get; set; }
}
