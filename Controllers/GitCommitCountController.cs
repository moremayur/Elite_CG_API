using Elite_CG_API.DataAccess;
using Elite_CG_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Elite_CG_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GitCommitCountController : ControllerBase
    {

        [HttpGet]
        [Route("CommitCount")]
        public async Task<ActionResult<IEnumerable<AuthorDetails>>> GetTotalCommitCount(DateTime FromDate, DateTime ToDate)
        {
            return Ok(await new CommitCountProvider().GetCommitCount(FromDate,ToDate));
        }

        [HttpGet]
        [Route("AverageCodingDays")]
        public async Task<ActionResult<double>> GetAverageCodingDays(DateTime FromDate, DateTime ToDate)
        {
            return Ok(await new CommitCountProvider().GetTotalAverageCodingDays(FromDate, ToDate));
        }

    }
}
