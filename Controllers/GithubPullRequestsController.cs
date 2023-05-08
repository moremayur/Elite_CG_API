using Elite_CG_API.DataAccess;
using Elite_CG_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Elite_CG_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GithubPullRequestsController: ControllerBase
    {
        private readonly ILogger<GithubPullRequestsController> _logger;

        public GithubPullRequestsController(ILogger<GithubPullRequestsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GitHubTimeToMergeMatrics")]
        public async Task<ActionResult<TimeToMerge>> GetTimetoMergeGitData(DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GitHubTimeToMergeMatrics - Called.");

            if (fromDate == DateTime.MinValue)
            {
                return BadRequest(new { error = "Invalid Input" });
            }

            return Ok(await new GithubPullRequestsProvider().GetTimeToMergePRData(fromDate, toDate));
        }

        [HttpGet]
        [Route("GitHubTimeToFirstCommentMatrics")]
        public async Task<ActionResult<List<TimeToFirstComment>>> GetTimetoFirstCommentGitData(DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GitHubTimeToFirstCommentMatrics - Called.");

            if (fromDate == DateTime.MinValue)
            {
                return BadRequest(new { error = "Invalid Input" });
            }

            return Ok(await new GithubPullRequestsProvider().GetTimeToFirstCommentPRData(fromDate, toDate));
        }
    }
}
