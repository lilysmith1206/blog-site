using ErrorOr;

namespace Lylink.Shared.Api.Client.Base;

public static class Errors
{
    private const string FailingHealthChecksKey = "healthChecks";

    extension(ErrorType)
    {
        public static ErrorType NotAvailable => (ErrorType)100;
    }

    extension(Error error)
    {
        public static Error NotAvailable(List<string> failingHealthChecks, string? description = null)
            => Error.Custom(
                (int)ErrorType.NotAvailable,
                "NOT_AVAILABLE",
                description ?? "The system requested is not available at this moment.",
                metadata: new()
                {
                    { FailingHealthChecksKey, failingHealthChecks }
                }
            );


        public List<string> FailingHealthChecks
        {
            get
            {
                if (!error.Metadata!.TryGetValue(FailingHealthChecksKey, out var value))
                    throw new ArgumentException("Error metadata does not have the failing health checks key: this is not correct usage of the error object.");
            
                if (value is not List<string> failingHealthChecks)
                    throw new InvalidCastException("The value under the failing health checks key is not a list of strings: bad data.");

                return failingHealthChecks;
            }
        }
    }
}