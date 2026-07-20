using System;
using System.Collections.Generic;

namespace Lylink.Database.Context.Models;

public partial class FailedVisitAnalytic
{
    public int Id { get; set; }

    public string SessionId { get; set; } = null!;

    public string AttemptedSlug { get; set; } = null!;

    public string RedirectedSlug { get; set; } = null!;

    public DateTime DateCreated { get; set; }
}
