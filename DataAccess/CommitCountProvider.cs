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

        internal async Task<Rootobject> GetCommitCount()
        {
            Rootobject rootobject = new Rootobject();
            using (var response = await new CommonData().Client.GetAsync(CommonData.Base_URL + "repos/"+CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/"+"commits"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                rootobject = JsonConvert.DeserializeObject<Rootobject>(apiResponse);
            }
            return rootobject;
        }

    }
}
