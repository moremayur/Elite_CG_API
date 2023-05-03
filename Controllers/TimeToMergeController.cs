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
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetTimetoMergeGitData()
        {
            var data = Task.Factory.StartNew(() =>
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            });

            return Ok(await data);
        }
    }
}
