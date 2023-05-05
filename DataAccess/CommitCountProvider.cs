using Elite_CG_API.Common;
using Elite_CG_API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elite_CG_API.DataAccess
{
    public class CommitCountProvider
    {
        public CommitCountProvider()
        {
        }

        internal async Task<IEnumerable<CommitCount>> GetCommitCount()
        {
            IEnumerable<CommitCount> result= new List<CommitCount>();
            using (var response = await new CommonData().Client.GetAsync(CommonData.Base_URL + "repos/"+CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/"+ "commits?since=2023-04-26T10:50:00z&until=2023-05-05T10:59:00z"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<IEnumerable<CommitCount>>(apiResponse);
            }
            return result ;
        }

    }
}
