using LylinkBackend.AnalyticsShared;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend.AnalyticsApi.Controllers;

public class AnalyticsController : Controller
{
    private readonly ILogger<AnalyticsController> _logger;
    private readonly IVisitAnalyticsRepository _repository;

    public AnalyticsController(ILogger<AnalyticsController> logger, IVisitAnalyticsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [Authorize(Policy = "create_analytics")]
    [HttpPost("/analytics/visitor")]
    public ActionResult<int> CreateAnalytics([FromBody] NewVisitorAnalytic analytic)
    {
        try
        {
            var analyticId = _repository.CreateVisitorAnalytic(new()
            {
                SlugGiven = analytic.VisitTarget,
                SlugVisited = analytic.VisitResult,
                VisitedOn = analytic.Date,
                VisitorId = string.Empty // To deal with an out-of-date visitor analytics structure
            });

            _logger.LogInformation("Analytic created: {id}", analyticId);

            return Created((string?)null, analyticId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception on adding visitor analytics.");

            return StatusCode(500);
        }

    }
}
