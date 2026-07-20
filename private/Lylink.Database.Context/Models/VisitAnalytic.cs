using System;
using System.Collections.Generic;

namespace Lylink.Database.Context.Models;

public partial class VisitAnalytic
{
    public int Id { get; set; }

    public string SessionId { get; set; } = null!;

    public string VisitedSlug { get; set; } = null!;

    public DateTime DateCreated { get; set; }
}
