using Elite_CG_API.DataAccess;
using Elite_CG_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elite_CG_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeToMergeController: ControllerBase
    {
        private readonly ILogger<TimeToMergeController> _logger;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public TimeToMergeController(ILogger<TimeToMergeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GitHubTimeToMergeMatrics")]
        public async Task<ActionResult<TimeToMerge>> GetTimetoMergeGitData(DateTime fromDate, DateTime toDate)
        {
            return Ok(await new TimeToMergeProvider().GetTimeToMergePRData(fromDate, toDate));
        }
    }
}
