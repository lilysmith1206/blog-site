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
    [HttpPost("/analytics/visit/success")]
    public ActionResult<int> CreateSuccessAnalytic([FromBody] NewSuccessVisitAnalytic analytic)
    {
        try
        {
            var analyticId = _repository.CreateSuccessVisitAnalytic(analytic.SessionId, analytic.Visited, analytic.Date);

            _logger.LogInformation("Analytic created: {id}", analyticId);

            return Created((string?)null, analyticId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception on adding visitor analytics.");

            return StatusCode(500);
        }
    }


    [Authorize(Policy = "create_analytics")]
    [HttpPost("/analytics/visit/failure")]
    public ActionResult<int> CreateFailureAnalytic([FromBody] NewFailureVisitAnalytic analytic)
    {
        try
        {
            var analyticId = _repository.CreateFailedVisitAnalytic(analytic.SessionId, analytic.Attempted, analytic.RedirectedTo, analytic.Date);

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
