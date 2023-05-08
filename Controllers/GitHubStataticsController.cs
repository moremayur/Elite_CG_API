using Elite_CG_API.DataAccess;
using Elite_CG_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elite_CG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubStataticsController : ControllerBase
    {
        private readonly ILogger<GitHubStataticsController> _logger;

        public GitHubStataticsController(ILogger<GitHubStataticsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GitHubStatatics")]
        public async Task<ActionResult<RepoStatatics>> GitHubStatatics(DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GitHubStatatics - Called.");

            if (fromDate == DateTime.MinValue)
            {
                return BadRequest(new { error = "Invalid Input" });
            }

            return Ok(await new GithubStataticsProvider().GetStataticsData(fromDate, toDate));
        }

        [HttpGet]
        [Route("GitHubStarContributors")]
        public async Task<ActionResult<List<GitRepoStarContributors>>> GitHubStarContributors()
        {
            _logger.LogInformation("GitHubStarContributors - Called.");

            return Ok(await new GithubStataticsProvider().GetStarContributors());
        }
    }
}
