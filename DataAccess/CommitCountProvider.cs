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

        //?since=2023-04-26T10:50:00z&until=2023-05-05T10:59:00z
        internal async Task<IEnumerable<AuthorDetails>> GetCommitCount(DateTime FromDate, DateTime ToDate)
        {
            IEnumerable<CommitCount> result = new List<CommitCount>();
            List<AuthorDetails> authorDetails = new List<AuthorDetails>();
            using (var response = await new CommonData().Client.GetAsync(CommonData.Base_URL + "repos/" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/" + "commits?since=" + FromDate.ToString("yyyy-MM-dd") + "+&until=" + ToDate.ToString("yyyy-MM-dd")))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<IEnumerable<CommitCount>>(apiResponse);
                if (result != null)
                {
                    authorDetails = AuthorWiseCommit(result);
                }

            }
            return authorDetails;
        }

        internal async Task<double> GetTotalAverageCodingDays(DateTime fromDate, DateTime toDate)
        {
            IEnumerable<CommitCount> result = new List<CommitCount>();
            List<AuthorDetails> authorDetails = new List<AuthorDetails>();
            using (var response = await new CommonData().Client.GetAsync(CommonData.Base_URL + "repos/" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/" + "commits?since=" + fromDate.ToString("yyyy-MM-dd") + "+&until=" + toDate.ToString("yyyy-MM-dd")))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<IEnumerable<CommitCount>>(apiResponse);

                double numberOfDays = toDate.Subtract(fromDate).TotalDays;

                return result.Count() / numberOfDays;

            }
        }


   
        private List<AuthorDetails> AuthorWiseCommit(IEnumerable<CommitCount> rootobject)
        {
            List<AuthorDetails> result = new List<AuthorDetails>();

            foreach (var roo in rootobject)
            {
                var commitdetail = new CommitDetails();
                if (!result.Any(r => r.loginId == roo.author.login))
                {

                    var author = new AuthorDetails();
                    author.email = roo.commit.author.email;
                    author.authorName = roo.commit.author.name;
                    author.loginId = roo.author.login;
                    author.commitCount = 1;
                    author.AuthorID = roo.author.id;
                    List<CommitDetails> commitDetails = new List<CommitDetails>();

                    commitdetail.SHA = roo.sha;
                    commitdetail.commiterName = roo.commit.committer.name;
                    commitdetail.CommitMessage = roo.commit.message;
                    commitdetail.commitDate = roo.commit.author.date;
                    commitDetails.Add(commitdetail);
                    author.commitDetails = commitDetails;
                    result.Add(author);
                }
                else
                {
                    var availableAuthor = result.Find(r => r.loginId == roo.author.login);
                    var availableCommitDetails = availableAuthor.commitDetails;
                    availableAuthor.commitCount = availableAuthor.commitCount + 1;
                    commitdetail.SHA = roo.sha;
                    commitdetail.commiterName = roo.commit.committer.name;
                    commitdetail.CommitMessage = roo.commit.message;
                    commitdetail.commitDate = roo.commit.author.date;
                    availableCommitDetails.Add(commitdetail);
                }
            }

            return result;

        }
    }
}