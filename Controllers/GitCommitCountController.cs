using Elite_CG_API.DataAccess;
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

        [HttpGet]
        [Route("CommitCount")]
        public async Task<ActionResult<Rootobject>> GetTotalCommitCount()
        {
            return Ok(await new CommitCountProvider().GetCommitCount());
        }

    }
}
