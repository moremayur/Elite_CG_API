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
            try
            {
                string apiURL = CommonData.Base_URL + "repos/" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/" + "commits";
                string apiResponse = await new CommonData().HttpCallHandler(apiURL);
                rootobject = JsonConvert.DeserializeObject<Rootobject>(apiResponse);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }

            return rootobject;
        }

    }
}
