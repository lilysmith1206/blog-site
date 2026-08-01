using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using static Lylink.Shared.Api.HealthChecks.CachingHealthChecksPublisher;

namespace Lylink.Shared.Api.HealthChecks;

[AttributeUsage(AttributeTargets.Method)]
public class RequireHealthyAttribute(params string[] checkNames) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var wrapper = context.HttpContext.RequestServices.GetRequiredService<HealthReportWrapper>();
        var unhealthy = checkNames.Where(wrapper.IsCheckUnhealthy);

        if (unhealthy.Any())
            context.Result = new ObjectResult(new { failedChecks = unhealthy })
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable
            };
        else
            await next();
    }
}