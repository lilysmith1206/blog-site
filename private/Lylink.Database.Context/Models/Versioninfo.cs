using System;
using System.Collections.Generic;

namespace Lylink.Database.Context.Models;

public partial class Versioninfo
{
    public long Version { get; set; }

    public DateTime? AppliedOn { get; set; }

    public string? Description { get; set; }
}
