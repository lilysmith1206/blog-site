using System;
using System.Collections.Generic;
using System.Text;

namespace Lylink.Shared.Api.Authentication;

public class BearerTokenValidationOptions
{
    public string? ExpectedAuthority { get; init; }

    public string? ExpectedAudience { get; init; }

    public bool? RequiresHttpsAuthority { get; init; }

    public bool? ValidateIssuer { get; init; }

    public bool? ValidateAudience { get; init; }

    public bool? ValidateLifetime { get; init; }
}
