using Elite_CG_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Elite_CG_API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GitCommitCountController : ControllerBase
    {
        private readonly ILogger<GitCommitCountController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public GitCommitCountController(ILogger<GitCommitCountController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [Route("CommitCount")]
        public async Task<ActionResult<IEnumerable<Class1>>> GetTotalCommitCount()
        {
            var accessToken = "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";
            List<Class1> commitDetails = new List<Class1>();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/moremayur/wk_cg_test/commits");

        var client = _clientFactory.CreateClient();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiString = await response.Content.ReadAsStringAsync();
                commitDetails = JsonConvert.DeserializeObject<List<Class1>>(apiString);
            }


            return commitDetails;
           
        }

    }
}
